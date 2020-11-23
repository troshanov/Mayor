namespace Mayor.Services.Data.Cities
{
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;

    public class CitiesService : ICitiesService
    {
        private readonly IRepository<City> repo;

        public CitiesService(IRepository<City> repo)
        {
            this.repo = repo;
        }

        public async Task<City> CreateAsync(string name)
        {
            var city = this.repo.All()
                .FirstOrDefault(c => c.Name == name);

            if (city != null)
            {
                return city;
            }

            city = new City
            {
                Name = name,
            };

            await this.repo.AddAsync(city);
            await this.repo.SaveChangesAsync();

            return city;
        }
    }
}
