namespace Mayor.Services.Data.Institutions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Mapping;
    using Mayor.Web.ViewModels.User;

    public class InstitutionsService : IInstitutionsService
    {
        private readonly IDeletableEntityRepository<Institution> institutionRepo;

        public InstitutionsService(
            IDeletableEntityRepository<Institution> institutionRepo)
        {
            this.institutionRepo = institutionRepo;
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
    }
}
