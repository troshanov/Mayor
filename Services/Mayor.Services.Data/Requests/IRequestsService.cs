namespace Mayor.Services.Data.Requests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Mayor.Web.ViewModels.Request;

    public interface IRequestsService
    {
        Task CreateAsync(RequestInputModel input, string userId, string rootPath);

        IList<T> GetAllByUserId<T>(string userId);

        T GetById<T>(int requestId);

        Task ApproveById(int id, string userId);

        Task DismissById(int id, string userId);
    }
}
