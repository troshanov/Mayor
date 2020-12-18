namespace Mayor.Services.Data.Institutions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Mayor.Data.Models;
    using Mayor.Web.ViewModels.User;

    public interface IInstitutionsService
    {
        Task CreateAsync(AppUserInputModel input, string userId);

        Institution GetByUserId(string userId);

        IEnumerable<T> GetTopTenByRating<T>();

        Institution GetBySolvedIssueId(int issueId);

        Task UpdateRating(int institutionId);

        Institution GetById(int id);

        int GetSolvedIssuesCountById(int id);

        int GetActiveIssuesCountById(int id);
    }
}
