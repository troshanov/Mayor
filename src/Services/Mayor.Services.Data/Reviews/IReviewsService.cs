using System.Threading.Tasks;

namespace Mayor.Services.Data.Reviews
{
    public interface IReviewsService
    {
        Task CreateAsync(int citizenId, int issueId, int score, string comment);

        bool HasReviewedIssue(int citizenId, int issueId);
    }
}
