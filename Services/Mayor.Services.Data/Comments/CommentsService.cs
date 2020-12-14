namespace Mayor.Services.Data.Comments
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Citizens;
    using Mayor.Services.Data.Institutions;
    using Mayor.Web.ViewModels.Comment;

    public class CommentsService : ICommentsService
    {
        private readonly IRepository<Comment> commentsRepo;
        private readonly ICitizensService citizensService;
        private readonly IInstitutionsService institutionsService;

        public CommentsService(
            IRepository<Comment> commentsRepo,
            ICitizensService citizensService,
            IInstitutionsService institutionsService)
        {
            this.commentsRepo = commentsRepo;
            this.citizensService = citizensService;
            this.institutionsService = institutionsService;
        }

        public async Task CreateAsync(CommentInputModel input)
        {
            var comment = new Comment
            {
                Content = input.Content,
                UserId = input.UserId,
                IssueId = input.IssueId,
            };

            await this.commentsRepo.AddAsync(comment);
            await this.commentsRepo.SaveChangesAsync();
        }

        public IEnumerable<CommentInListViewModel> GetAllByIssueId(int page, int issueId, int itemsPerPage = 12)
        {
            return this.commentsRepo.All()
                .Where(c => c.IssueId == issueId)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(c => new CommentInListViewModel
                {
                    Content = c.Content,
                    UserName = this.citizensService.GetByUserId(c.UserId) == null ?
                                this.institutionsService.GetByUserId(c.UserId).Name :
                                this.citizensService.GetByUserId(c.UserId).FirstName +
                                " " +
                                this.citizensService.GetByUserId(c.UserId).LastName,
                    UserId = c.UserId,
                    CreatedOn = c.CreatedOn.ToString("d"),
                    UserAvatarUrl = c.User.Pictures.Any(p => p.IssueId == null) ? "/img/" + c.User.Pictures
                                    .FirstOrDefault(p => p.IssueId == null).Id +
                                    c.User.Pictures.FirstOrDefault(p => p.IssueId == null).Extension
                                    : "/img/anon.png",
                })
                .ToList();
        }

        public int GetCountByIssueId(int issueId)
        {
            return this.commentsRepo.All()
                .Where(c => c.IssueId == issueId)
                .Count();
        }
    }
}
