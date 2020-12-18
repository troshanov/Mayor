namespace Mayor.Services.Data.Reviews
{
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;

    public class ReviewsService : IReviewsService
    {
        private readonly IRepository<IssueReview> reviewsReop;

        public ReviewsService(
            IRepository<IssueReview> reviewsReop)
        {
            this.reviewsReop = reviewsReop;
        }

        public async Task CreateAsync(int citizenId, int issueId, int score, string comment)
        {
            await this.reviewsReop.AddAsync(new IssueReview
            {
                CitizenId = citizenId,
                IssueId = issueId,
                Score = score,
                Comment = comment,
            });
            await this.reviewsReop.SaveChangesAsync();
        }

        public bool HasReviewedIssue(int citizenId, int issueId)
        {
            return this.reviewsReop
                .AllAsNoTracking()
                .Any(ir => ir.CitizenId == citizenId && ir.IssueId == issueId);
        }
    }
}
