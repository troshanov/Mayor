namespace Mayor.Services.Data.Votes
{
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Citizens;
    using Mayor.Services.Data.Issues;

    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> votesRepo;
        private readonly ICitizensService citizensService;
        private readonly IIssuesService issuesService;

        public VotesService(
            IRepository<Vote> votesRepo,
            ICitizensService citizensService,
            IIssuesService issuesService)
        {
            this.votesRepo = votesRepo;
            this.citizensService = citizensService;
            this.issuesService = issuesService;
        }

        public async Task CreateAsync(string userId, int issueId)
        {
            var citizenId = this.citizensService.GetByUserId(userId).Id;
            await this.votesRepo.AddAsync(new Vote
            {
                CitizenId = citizenId,
                IssueId = issueId,
            });
            await this.votesRepo.SaveChangesAsync();
        }

        public bool HasVoted(string userId, int issueId)
        {
            var citizenId = this.citizensService.GetByUserId(userId).Id;
            if (this.votesRepo.AllAsNoTracking().Any(v => v.CitizenId == citizenId && v.IssueId == issueId))
            {
                return true;
            }

            return false;
        }

        public async Task DeleteAsync(string userId, int issueId)
        {
            var citizenId = this.citizensService.GetByUserId(userId).Id;
            var voteToDelete = this.votesRepo.All().FirstOrDefault(v => v.CitizenId == citizenId && v.IssueId == issueId);
            this.votesRepo.Delete(voteToDelete);
            await this.votesRepo.SaveChangesAsync();
        }
    }
}
