using Mayor.Services.Data.Categories;
using Mayor.Services.Data.Issues;
using Mayor.Web.ViewModels.Issue;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mayor.Web.Controllers
{
    public class IssuesController : Controller
    {
        private readonly IIssuesService issuesService;
        private readonly ICategoriesService categoriesService;

        public IssuesController(
            IIssuesService issuesService,
            ICategoriesService categoriesService)
        {
            this.issuesService = issuesService;
            this.categoriesService = categoriesService;
        }

        public IActionResult Create()
        {
            var viewModel = new CreateIssueInputModel();
            viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateIssueInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                return this.View();
            }

            await this.issuesService.CreateAsync(input);

            return this.Redirect("/");
        }
    }
}
