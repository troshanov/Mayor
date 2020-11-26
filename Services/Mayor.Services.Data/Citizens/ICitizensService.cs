namespace Mayor.Services.Data.Citizens
{
    using System.Threading.Tasks;

    using Mayor.Data.Models;
    using Mayor.Web.ViewModels.User;

    public interface ICitizensService
    {
        Task CreateAsync(AppUserInputModel input, string userId);

        Citizen GetByUserId(string userId);
    }
}
