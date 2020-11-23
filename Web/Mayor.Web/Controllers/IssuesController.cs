using Mayor.Services.Data.Categories;
using Mayor.Services.Data.Issues;
using Mayor.Web.ViewModels.Issue;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mayor.Web.Controllers
{
    public class IssuesController : Controller
    {
        private readonly IIssuesService issuesService;
        private readonly ICategoriesService categoriesService;
        private readonly IWebHostEnvironment environment;

        public IssuesController(
            IIssuesService issuesService,
            ICategoriesService categoriesService,
            IWebHostEnvironment environment)
        {
            this.issuesService = issuesService;
            this.categoriesService = categoriesService;
            this.environment = environment;
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

            var rootPath = $"{this.environment.WebRootPath}/img";
            await this.issuesService.CreateAsync(input, rootPath);

            return this.Redirect("/");
        }
    }
}
