namespace Mayor.Services.Data.Issues
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Addresses;
    using Mayor.Services.Data.Citizens;
    using Mayor.Services.Data.IssueTags;
    using Mayor.Services.Data.Pictures;
    using Mayor.Services.Mapping;
    using Mayor.Web.ViewModels.Issue;

    public class IssuesService : IIssuesService
    {
        private readonly string[] allowedAttachmentExtensions = new[] { "docx", "pdf" };
        private readonly IDeletableEntityRepository<Issue> issuesRepo;
        private readonly IRepository<IssueAttachment> issueAttRepo;
        private readonly IDeletableEntityRepository<Attachment> attRepo;
        private readonly IDeletableEntityRepository<Citizen> citizenRepo;
        private readonly ICitizensService citizensService;
        private readonly IPicturesService picturesService;
        private readonly IAddressesService addressesService;
        private readonly IIssueTagsService issueTagsService;

        public IssuesService(
            IDeletableEntityRepository<Issue> issuesRepo,
            IRepository<IssueAttachment> issueAttRepo,
            IDeletableEntityRepository<Attachment> attRepo,
            IDeletableEntityRepository<Citizen> citizenRepo,
            ICitizensService citizensService,
            IPicturesService picturesService,
            IAddressesService addressesService,
            IIssueTagsService issueTagsService)
        {
            this.issuesRepo = issuesRepo;
            this.issueAttRepo = issueAttRepo;
            this.attRepo = attRepo;
            this.citizenRepo = citizenRepo;
            this.citizensService = citizensService;
            this.picturesService = picturesService;
            this.addressesService = addressesService;
            this.issueTagsService = issueTagsService;
        }

        public async Task CreateAsync(CreateIssueInputModel input, string userId, string rootPath)
        {
            var citizenId = this.citizensService.GetByUserId(userId).Id;
            var address = await this.addressesService.CreateAsync(input.Address);
            var issue = new Issue
            {
                Title = input.Title,
                Description = input.Description,
                Address = address,
                CategoryId = input.CategoryId,
                StatusId = 2,
                CreatorId = citizenId,
            };

            // Add image to file system
            issue.Pictures.Add(await this.picturesService.CreateFileAsync(userId, rootPath, input.TitlePicture));

            // Add Attachments to file system
            if (input.Attachments != null)
            {
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
                    var physicalPath = $"{rootPath}/att/issues/{attachment.Id}{attachment.Extension}";
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
            }

            await this.issuesRepo.AddAsync(issue);
            await this.issuesRepo.SaveChangesAsync();
            await this.issueTagsService.CraeteAsync(issue.Id, input.Tags);
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 12)
        {
            // TODO: Order items
            return this.issuesRepo.AllAsNoTracking()
                .Where(i => i.Pictures.Any())
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetAllByCategoryName<T>(int page, string category, int itemsPerPage = 12)
        {
            return this.issuesRepo.AllAsNoTracking()
                .Where(i => i.Category.Name == category)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetAllByUserId<T>(int page, string userId, int itemsPerPage = 12)
        {
            // Use CitizensService instead
            var citizenId = this.citizenRepo.All()
                .Where(c => c.UserId == userId)
                .Select(c => c.Id)
                .FirstOrDefault();

            return this.issuesRepo.AllAsNoTracking()
                .Where(i => i.CreatorId == citizenId)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToList();
        }

        public T GetById<T>(int id)
        {
            return this.issuesRepo.All()
                .Where(i => i.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public int GetCount()
        {
            return this.issuesRepo.AllAsNoTracking().Count();
        }

        public int GetCountByCateogry(string category)
        {
            return this.issuesRepo.AllAsNoTracking()
                .Where(i => i.Category.Name == category)
                .Count();
        }

        public int GetCountByUserId(string userId)
        {
            var citizenId = this.citizenRepo.AllAsNoTracking()
                .Where(c => c.UserId == userId)
                .Select(c => c.Id)
                .FirstOrDefault();

            return this.issuesRepo.AllAsNoTracking()
                .Where(i => i.CreatorId == citizenId)
                .Count();
        }
    }
}
