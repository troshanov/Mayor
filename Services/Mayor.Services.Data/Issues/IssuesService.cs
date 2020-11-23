using Mayor.Data.Common.Repositories;
using Mayor.Data.Models;
using Mayor.Services.Data.Addresses;
using Mayor.Services.Data.IssueTags;
using Mayor.Web.ViewModels.Issue;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mayor.Services.Data.Issues
{
    public class IssuesService : IIssuesService
    {
        private readonly string[] allowedImageExtensions = new[] { "jpg", "png" };
        private readonly string[] allowedAttachmentExtensions = new[] { "docx", "pdf" };
        private readonly IDeletableEntityRepository<Issue> issuesRepo;
        private readonly IRepository<IssueAttachment> issueAttRepo;
        private readonly IDeletableEntityRepository<Attachment> attRepo;
        private readonly IAddressesService addressesService;
        private readonly IIssueTagsService issueTagsService;

        public IssuesService(
            IDeletableEntityRepository<Issue> issuesRepo,
            IRepository<IssueAttachment> issueAttRepo,
            IDeletableEntityRepository<Attachment> attRepo,
            IAddressesService addressesService,
            IIssueTagsService issueTagsService)
        {
            this.issuesRepo = issuesRepo;
            this.issueAttRepo = issueAttRepo;
            this.attRepo = attRepo;
            this.addressesService = addressesService;
            this.issueTagsService = issueTagsService;
        }

        public async Task CreateAsync(CreateIssueInputModel input, string rootPath)
        {
            var address = await this.addressesService.CreateAsync(input.Address);
            var issue = new Issue
            {
                Title = input.Title,
                Description = input.Description,
                Address = address,
                CategoryId = input.CategoryId,
                StatusId = 2,
                CreatorId = 5,
            };

            // Add the Image to file system
            var imgExtension = Path.GetExtension(input.TitlePicture.FileName);
            if (!this.allowedImageExtensions.Any(x => imgExtension.EndsWith(x)))
            {
                throw new Exception($"Format should be .jpg or .png!");
            }

            var userId = "a8be01d1-f291-4fa3-a697-2dbc30dbc8a6";
            var picture = new Picture
            {
                AddedByUserId = userId,
                Issue = issue,
                Extension = imgExtension,
            };
            issue.Pictures.Add(picture);

            Directory.CreateDirectory($"{rootPath}/img/issues/");
            var physicalPath = $"{rootPath}/img/issues/{picture.Id}{picture.Extension}";
            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await input.TitlePicture.CopyToAsync(fileStream);

            // Add Attachments to file system
            foreach (var att in input.Attachments)
            {
                var attExtension = Path.GetExtension(att.FileName);
                if (!this.allowedAttachmentExtensions.Any(x => attExtension.EndsWith(x)))
                {
                    throw new Exception($"Format should be .docx or .pdf!");
                }

                var attachment = new Attachment
                {
                    AddedByUserId = userId,
                    Extension = attExtension,
                };

                Directory.CreateDirectory($"{rootPath}/att/issues/");
                physicalPath = $"{rootPath}/att/issues/{attachment.Id}{attachment.Extension}";
                using Stream attFileStream = new FileStream(physicalPath, FileMode.Create);
                await att.CopyToAsync(attFileStream);
                await this.attRepo.AddAsync(attachment);

                var issueAttachment = new IssueAttachment
                {
                    Attachment = attachment,
                    Issue = issue,
                };

                await this.issueAttRepo.AddAsync(issueAttachment);
            }

            await this.issuesRepo.AddAsync(issue);
            await this.issuesRepo.SaveChangesAsync();
            await this.issueTagsService.CraeteAsync(issue.Id, input.Tags);
        }
    }
}
