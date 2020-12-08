using Mayor.Web.ViewModels.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayor.Services.Data.Requests
{
    public interface IRequestsService
    {
        Task CreateAsync(RequestInputModel input, string userId, string rootPath);

        IList<T> GetAllByUserId<T>(string userId);

        T GetById<T>(int requestId);

        Task ApproveById(int id);

        Task DismissById(int id);
    }
}
