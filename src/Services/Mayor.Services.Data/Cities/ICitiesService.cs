namespace Mayor.Services.Data.Cities
{
    using System.Threading.Tasks;

    using Mayor.Data.Models;

    public interface ICitiesService
    {
        Task<City> CreateAsync(string name);
    }
}
