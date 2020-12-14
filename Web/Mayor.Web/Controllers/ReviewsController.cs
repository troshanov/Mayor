namespace Mayor.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Mayor.Services.Data.Citizens;
    using Mayor.Services.Data.Institutions;
    using Mayor.Services.Data.Issues;
    using Mayor.Services.Data.Requests;
    using Mayor.Services.Data.Reviews;
    using Mayor.Web.ViewModels.Request;
    using Mayor.Web.ViewModels.Review;
    using Microsoft.AspNetCore.Mvc;

    public class ReviewsController : Controller
    {
        private readonly IRequestsService requestsService;
        private readonly IIssuesService issuesService;
        private readonly IReviewsService reviewsService;
        private readonly ICitizensService citizensService;
        private readonly IInstitutionsService institutionsService;

        public ReviewsController(
            IRequestsService requestsService,
            IIssuesService issuesService,
            IReviewsService reviewsService,
            ICitizensService citizensService,
            IInstitutionsService institutionsService) 
        {
            this.requestsService = requestsService;
            this.issuesService = issuesService;
            this.reviewsService = reviewsService;
            this.citizensService = citizensService;
            this.institutionsService = institutionsService;
        }

        public IActionResult New(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var citizenId = this.citizensService.GetByUserId(userId).Id;

            var viewModel = new ReviewInputModel
            {
                IssueId = id,
                SolveRequest = this.requestsService.GetById<SingleRequestViewModel>(this.issuesService.GetSolvedRequestIdById(id)),
                HasReviewed = this.reviewsService.HasReviewedIssue(citizenId, id),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> New(ReviewInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var citizenId = this.citizensService.GetByUserId(userId).Id;
            if (this.reviewsService.HasReviewedIssue(citizenId,input.IssueId))
            {
                this.ModelState.AddModelError("HasReviewed", "You have already reviewed this issue!");
            }

            if (!this.ModelState.IsValid)
            {
                input.SolveRequest = this.requestsService.GetById<SingleRequestViewModel>(this.issuesService.GetSolvedRequestIdById(input.IssueId));
                return this.View(input);
            }

            await this.reviewsService.CreateAsync(citizenId, input.IssueId, input.Score, input.Comment);
            var institutionId = this.institutionsService.GetBySolvedIssueId(input.IssueId).Id;
            await this.institutionsService.UpdateRating(institutionId);

            // TODO: Redirect to Reviewed issue's page
            return this.Redirect("/");
        }
    }
}
