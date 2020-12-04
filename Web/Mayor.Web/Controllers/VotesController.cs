using Mayor.Services.Data.Issues;
using Mayor.Services.Data.Votes;
using Mayor.Web.ViewModels.Vote;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mayor.Web.Controllers
{
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

        // TODO: Workaround for the AFT.
        [HttpPost]
        [IgnoreAntiforgeryToken]
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
