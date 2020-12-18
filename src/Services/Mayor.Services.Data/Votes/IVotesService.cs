namespace Mayor.Services.Data.Votes
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        Task CreateAsync(string userId, int issueId);

        bool HasVoted(string userId, int issueId);

        Task DeleteAsync(string userId, int issueId);
    }
}
