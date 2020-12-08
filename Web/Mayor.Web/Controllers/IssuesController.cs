namespace Mayor.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Mayor.Services.Data.Attachments;
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
        private readonly IAttachmentsService attService;
        private readonly IWebHostEnvironment environment;

        public IssuesController(
            IIssuesService issuesService,
            ICategoriesService categoriesService,
            IAttachmentsService attService,
            IWebHostEnvironment environment)
        {
            this.issuesService = issuesService;
            this.categoriesService = categoriesService;
            this.attService = attService;
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
                var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await this.issuesService.CreateAsync(input, userId, rootPath);
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

        [Route("Issues/{id:int}")]
        public IActionResult Single(int id)
        {
            var viewModel = this.issuesService.GetById<SingleIssueViewModel>(id);

            viewModel.SidebarIssues = this.issuesService
                .GetAllByCategoryName<IssueInSidebarViewModel>(1, viewModel.CategoryName, 5)
                .OrderByDescending(vm => vm.VotesCount)
                .ToList();

            return this.View(viewModel);
        }

        public IActionResult My(int id = 1)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var viewModel = new IssueListViewModel
            {
                PageNumber = id,
                ItemsPerPage = 6,
                IssuesCount = this.issuesService.GetCountByUserId(userId),
                Issues = this.issuesService.GetAllByUserId<IssueInListViewModel>(id, userId, 6),
            };

            return this.View(viewModel);
        }

        public IActionResult Download(string id)
        {
            var attachment = this.attService.GetFileById(id);
            var rootPath = $"{this.environment.WebRootPath}";
            var pathToFile = rootPath + "/att/issues/" + attachment.Id + attachment.Extension;
            var fileStream = new FileStream(pathToFile, FileMode.Open);

            var contetType = attachment.Extension switch
            {
                ".jpg" => "image/jpeg",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".png" => "image/png",
                ".pdf" => "application/pdf",
                _ => null,
            };
            this.HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{attachment.Id}{attachment.Extension}\" ");
            return this.File(fileStream, contetType);
        }
    }
}
