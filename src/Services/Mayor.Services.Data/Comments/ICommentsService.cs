namespace Mayor.Services.Data.Comments
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Mayor.Web.ViewModels.Comment;

    public interface ICommentsService
    {
        Task CreateAsync(CommentInputModel input);

        IEnumerable<CommentInListViewModel> GetAllByIssueId(int page, int issueId, int itemsPerPage = 12);

        int GetCountByIssueId(int issueId);

        int GetTotalCommentsByUserId(string userId);
    }
}
