namespace Mayor.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Mayor.Services.Data.Issues;
    using Mayor.Services.Data.Votes;
    using Mayor.Web.ViewModels.Vote;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : BaseController
    {
        private readonly IVotesService votesService;
        private readonly IIssuesService issuesService;

        public VotesController(
            IVotesService votesService,
            IIssuesService issuesService)
        {
            this.votesService = votesService;
            this.issuesService = issuesService;
        }

        [HttpPost]
        public async Task<ActionResult<PostVoteResponseModel>> Post(PostVoteInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!this.votesService.HasVoted(userId, input.IssueId))
            {
                await this.votesService.CreateAsync(userId, input.IssueId);
            }
            else
            {
                await this.votesService.DeleteAsync(userId, input.IssueId);
            }

            var viewModel = this.issuesService.GetById<PostVoteResponseModel>(input.IssueId);
            return viewModel;
        }
    }
}
