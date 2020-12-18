namespace Mayor.Services.Data.Requests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Attachments;
    using Mayor.Services.Data.Citizens;
    using Mayor.Services.Data.Institutions;
    using Mayor.Services.Data.Issues;
    using Mayor.Services.Mapping;
    using Mayor.Web.ViewModels.Request;
    using Microsoft.EntityFrameworkCore;

    public class RequestsService : IRequestsService
    {
        private readonly IDeletableEntityRepository<IssueRequest> requestsRepo;
        private readonly IAttachmentsService attService;
        private readonly IInstitutionsService institutionsService;
        private readonly ICitizensService citizensService;
        private readonly IIssuesService issuesService;
        private readonly IDeletableEntityRepository<Issue> issuesRepo;
        private readonly IRepository<IssueRequestAttachment> issueRequestAttsService;
        private readonly IDeletableEntityRepository<Attachment> attRepo;

        public RequestsService(
            IDeletableEntityRepository<IssueRequest> requestsRepo,
            IAttachmentsService attService,
            IInstitutionsService institutionsService,
            ICitizensService citizensService,
            IIssuesService issuesService,
            IDeletableEntityRepository<Issue> issuesRepo,
            IRepository<IssueRequestAttachment> issueRequestAttsService,
            IDeletableEntityRepository<Attachment> attRepo)
        {
            this.requestsRepo = requestsRepo;
            this.attService = attService;
            this.institutionsService = institutionsService;
            this.citizensService = citizensService;
            this.issuesService = issuesService;
            this.issuesRepo = issuesRepo;
            this.issueRequestAttsService = issueRequestAttsService;
            this.attRepo = attRepo;
        }

        public async Task ApproveById(int id, string userId)
        {
            var request = this.requestsRepo.All()
                .FirstOrDefault(r => r.Id == id);

            var ownerId = this.issuesRepo.AllAsNoTracking()
                .Include(i => i.Creator)
                .FirstOrDefault(i => i.Id == request.IssueId).Creator.UserId;

            if (ownerId != userId)
            {
                return;
            }

            bool? isApproved = true;
            request.IsApproved = isApproved;

            if (request.IsSolveRequest)
            {
               await this.issuesService.UpdateStatusById(request.IssueId, 4, request.RequesterId);
            }
            else
            {
               await this.issuesService.UpdateStatusById(request.IssueId, 3);
            }

            this.requestsRepo.Update(request);
            await this.requestsRepo.SaveChangesAsync();
        }

        public async Task CreateAsync(RequestInputModel input, string userId, string rootPath)
        {
            var requesterId = this.institutionsService.GetByUserId(userId).Id;

            var request = new IssueRequest
            {
                IssueId = input.IssueId,
                RequesterId = requesterId,
                IsApproved = null,
                IsSolveRequest = input.IsSolveRequest,
                Description = input.Description,
            };

            if (input.Attachments != null && input.Attachments.Any())
            {
                foreach (var att in input.Attachments)
                {
                    var attachment = await this.attService.CreateAsync(userId, rootPath, att);
                    var requestAttachment = new IssueRequestAttachment
                    {
                        Attachment = attachment,
                        IssueRequest = request,
                    };

                    await this.issueRequestAttsService.AddAsync(requestAttachment);
                    await this.attRepo.AddAsync(attachment);
                }
            }

            await this.requestsRepo.AddAsync(request);
            await this.requestsRepo.SaveChangesAsync();
        }

        public async Task DismissById(int id, string userId)
        {
            var request = this.requestsRepo.All()
                .Where(r => r.Id == id)
                .FirstOrDefault();

            var ownerId = this.issuesRepo.AllAsNoTracking()
                .Include(i => i.Creator)
                .FirstOrDefault(i => i.Id == request.IssueId).Creator.UserId;

            if (ownerId != userId)
            {
                return;
            }

            request.IsApproved = false;
            this.requestsRepo.Delete(request);
            await this.requestsRepo.SaveChangesAsync();
        }

        public IList<T> GetAllByUserId<T>(string userId)
        {
            var citizenId = this.citizensService.GetByUserId(userId).Id;

            return this.requestsRepo.AllAsNoTracking()
                .Where(r => r.Issue.CreatorId == citizenId && r.IsApproved == null)
                .To<T>()
                .ToList();
        }

        public T GetById<T>(int requestId)
        {
            return this.requestsRepo.All()
                .Where(r => r.Id == requestId)
                .To<T>()
                .FirstOrDefault();
        }
    }
}
