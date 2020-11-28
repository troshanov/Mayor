namespace Mayor.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Mayor.Services.Data.Categories;
    using Mayor.Services.Data.Issues;
    using Mayor.Web.ViewModels.Issue;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class IssuesController : Controller
    {
        private const int ItemsPerPage = 12;
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

        [Authorize(Roles = "Citizen, Administrator")]
        public IActionResult Create()
        {
            var viewModel = new CreateIssueInputModel();
            viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
            return this.View(viewModel);
        }

        [HttpPost]

        [Authorize(Roles = "Citizen, Administrator")]
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

        public IActionResult All(int id = 1)
        {
            var viewModel = new IssueListViewModel
            {
                PageNumber = id,
                ItemsPerPage = ItemsPerPage,
                IssuesCount = this.issuesService.GetCount(),
                Issues = this.issuesService.GetAll<IssueInListViewModel>(id, ItemsPerPage),
            };
            return this.View(viewModel);
        }

        [Route("Issues/Category/{name}/{id?}")]
        public IActionResult Category(string name, int id = 1)
        {
            this.ViewData["Category"] = name;
            var viewModel = new IssueListViewModel
            {
                PageNumber = id,
                ItemsPerPage = ItemsPerPage,
                IssuesCount = this.issuesService.GetCountByCateogry(name),
                Issues = this.issuesService.GetAllByCategoryName<IssueInListViewModel>(id, name, ItemsPerPage),
            };
            return this.View(viewModel);
        }

        [Route("Issues/{id}")]
        public IActionResult Single(int id)
        {
            var viewModel = this.issuesService.GetById<SingleIssueViewModel>(id);
            return this.View(viewModel);
        }
    }
}
