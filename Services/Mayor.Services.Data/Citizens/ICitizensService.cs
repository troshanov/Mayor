using Mayor.Web.ViewModels.User;
using System.Threading.Tasks;

namespace Mayor.Services.Data.Citizens
{
    public interface ICitizensService
    {
        Task CreateAsync(AppUserInputModel input, string userId);
    }
}
