namespace Mayor.Services.Data.Citizens
{
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Web.ViewModels.User;
    using Microsoft.EntityFrameworkCore;

    public class CitizensService : ICitizensService
    {
        private readonly IDeletableEntityRepository<Citizen> citizenRepo;

        public CitizensService(
            IDeletableEntityRepository<Citizen> citizenRepo)
        {
            this.citizenRepo = citizenRepo;
        }

        public async Task CreateAsync(AppUserInputModel input, string userId)
        {
            var citizen = new Citizen
            {
                UserId = userId,
                FirstName = input.FirstName,
                LastName = input.LastName,
                Birthdate = input.Birthdate?.ToUniversalTime(),
                Sex = input.Sex,
            };

            await this.citizenRepo.AddAsync(citizen);
            await this.citizenRepo.SaveChangesAsync();
        }

        public Citizen GetById(int id)
        {
            return this.citizenRepo.All()
                .FirstOrDefault(c => c.Id == id);
        }

        public Citizen GetByUserId(string userId)
        {
            return this.citizenRepo.All()
                .FirstOrDefault(c => c.UserId == userId);
        }
    }
}
