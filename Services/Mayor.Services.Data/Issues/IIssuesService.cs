namespace Mayor.Services.Data.Issues
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Mayor.Web.ViewModels.Issue;

    public interface IIssuesService
    {
        Task CreateAsync(CreateIssueInputModel input, string userId, string imagesPath);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 12);

        IEnumerable<T> GetAllByCategoryName<T>(int page, string category, int itemsPerPage = 12);

        IEnumerable<T> GetAllByUserId<T>(int page, string userId, int itemsPerPage = 12);

        int GetCount();

        T GetById<T>(int id);

        int GetCountByCateogry(string category);

        int GetCountByUserId(string userId);

        Task UpdateStatusById(int issueId, int statusId, int requesterId = 0);

        int GetSolvedRequestIdById(int issueId);

        IEnumerable<T> GetTopTen<T>();

        Task DeleteById(int id, string userId);

        int GetAllIssueVotesCountByUserId(int id);
    }
}
