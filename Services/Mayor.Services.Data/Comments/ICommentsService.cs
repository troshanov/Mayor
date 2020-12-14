using Mayor.Web.ViewModels.Comment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayor.Services.Data.Comments
{
    public interface ICommentsService
    {
        Task CreateAsync(CommentInputModel input);

        IEnumerable<CommentInListViewModel> GetAllByIssueId(int page, int issueId, int itemsPerPage = 12);

        int GetCountByIssueId(int issueId);
    }
}
