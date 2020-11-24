namespace Mayor.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Mayor.Services.Data.Categories;
    using Mayor.Services.Data.Issues;
    using Mayor.Web.ViewModels.Issue;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

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
                return this.View(input);
            }

            var rootPath = $"{this.environment.WebRootPath}";

            try
            {
                await this.issuesService.CreateAsync(input, rootPath);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            return this.Redirect("/");
        }
    }
}
