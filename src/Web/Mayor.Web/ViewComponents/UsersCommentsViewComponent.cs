using Mayor.Services.Data.Comments;
using Mayor.Web.ViewModels.Comment;
using Microsoft.AspNetCore.Mvc;

namespace Mayor.Web.ViewComponents
{
    public class UsersCommentsViewComponent : ViewComponent
    {
        private const int ItemsPerPage = 3;
        private readonly ICommentsService commentsService;

        public UsersCommentsViewComponent(
            ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        public IViewComponentResult Invoke(int page, int issueId)
        {

            var viewModel = new ListOfCommentsViewModel
            {
                Comments = this.commentsService.GetAllByIssueId(page, issueId, ItemsPerPage),
            };

            return this.View(viewModel);
        }
    }
}
