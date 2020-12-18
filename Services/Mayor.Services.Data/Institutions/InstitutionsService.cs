namespace Mayor.Services.Data.Institutions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Mapping;
    using Mayor.Web.ViewModels.User;
    using Microsoft.EntityFrameworkCore;

    public class InstitutionsService : IInstitutionsService
    {
        private readonly IDeletableEntityRepository<Institution> institutionRepo;
        private readonly IRepository<IssueReview> reviewsRepo;
        private readonly IDeletableEntityRepository<Issue> issuesRepo;

        public InstitutionsService(
            IDeletableEntityRepository<Institution> institutionRepo,
            IRepository<IssueReview> reviewsRepo,
            IDeletableEntityRepository<Issue> issuesRepo)
        {
            this.institutionRepo = institutionRepo;
            this.reviewsRepo = reviewsRepo;
            this.issuesRepo = issuesRepo;
        }

        public async Task CreateAsync(AppUserInputModel input, string userId)
        {
            var institution = new Institution
            {
                UserId = userId,
                Name = input.Name,
                WebsiteUrl = input.Website,
                IsGovernment = input.IsGovernment == true ? true : false,
            };

            await this.institutionRepo.AddAsync(institution);
            await this.institutionRepo.SaveChangesAsync();
        }

        public Institution GetById(int id)
        {
            return this.institutionRepo.All()
                .FirstOrDefault(i => i.Id == id);
        }

        public int GetSolvedIssuesCountById(int id)
        {
            return this.issuesRepo.AllAsNoTrackingWithDeleted()
                .Where(i => i.SolverId != null && i.SolverId == id)
                .Count();
        }

        public Institution GetBySolvedIssueId(int issueId)
        {
            var institutionId = this.issuesRepo.AllAsNoTracking()
                .FirstOrDefault(i => i.Id == issueId).SolverId;

            return this.institutionRepo.All()
                .FirstOrDefault(i => i.Id == institutionId);
        }

        public Institution GetByUserId(string userId)
        {
            return this.institutionRepo.All()
                .FirstOrDefault(i => i.UserId == userId);
        }

        public IEnumerable<T> GetTopTenByRating<T>()
        {
            return this.institutionRepo.AllAsNoTracking()
                .OrderByDescending(i => i.Rating)
                .Take(10)
                .To<T>()
                .ToList();
        }

        public async Task UpdateRating(int institutionId)
        {
            var rating = this.reviewsRepo.AllAsNoTracking()
                .Include(r => r.Issue)
                .Where(r => r.Issue.SolverId == institutionId)
                .Average(r => r.Score);

            var institution = await this.institutionRepo.All()
                .FirstOrDefaultAsync(i => i.Id == institutionId);

            institution.Rating = (decimal)rating;
            await this.institutionRepo.SaveChangesAsync();
        }

        public int GetActiveIssuesCountById(int id)
        {
            return this.issuesRepo.AllAsNoTracking()
                .Where(i => i.StatusId == 3 && i.IssueRequests.Any(ir => ir.IsApproved == true && ir.RequesterId == id))
                .Count();
        }
    }
}
