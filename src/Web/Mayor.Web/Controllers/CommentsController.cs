namespace Mayor.Web.Controllers
{
    using System.Threading.Tasks;

    using Mayor.Services.Data.Comments;
    using Mayor.Web.ViewComponents;
    using Mayor.Web.ViewModels.Comment;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentsService commentsService;

        public CommentsController(
            ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommentInputModel commentInput)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect($"/Issues/{commentInput.IssueId}");
            }

            await this.commentsService.CreateAsync(commentInput);

            return this.Redirect($"/Issues/{commentInput.IssueId}");
        }

        [HttpPost]
        public IActionResult GetComments([FromBody] IssueCommentsInputModel input)
        {
            return this.ViewComponent(typeof(UsersCommentsViewComponent), new { page = input.Page, issueId = input.IssueId });
        }
    }
}
