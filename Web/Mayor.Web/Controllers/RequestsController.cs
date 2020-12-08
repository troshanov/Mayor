using Mayor.Data;
using Mayor.Services.Data.Attachments;
using Mayor.Services.Data.Requests;
using Mayor.Web.ViewModels.Request;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mayor.Web.Controllers
{
    public class RequestsController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly IRequestsService requestsService;
        private readonly IAttachmentsService attService;
        private readonly ApplicationDbContext dbContext;

        public RequestsController(
            IWebHostEnvironment environment,
            IRequestsService requestsService,
            IAttachmentsService attService,
            ApplicationDbContext dbContext)
        {
            this.environment = environment;
            this.requestsService = requestsService;
            this.attService = attService;
            this.dbContext = dbContext;
        }

        public IActionResult New(int id)
        {
            var viewModel = new RequestInputModel
            {
                IssueId = id,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> New(RequestInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var rootPath = $"{this.environment.WebRootPath}";

            try
            {
                var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await this.requestsService.CreateAsync(input, userId, rootPath);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(input);
            }

            // TODO: redirect to the Issue itself
            return this.Redirect("/");
        }

        public IActionResult All()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var viewModel = new RequestListViewModel
            {
                Requests = this.requestsService.GetAllByUserId<RequestInListViewModel>(userId),
            };
            return this.View(viewModel);
        }

        [Route("Requests/{id:int}")]
        public IActionResult Single(int id)
        {
            this.ViewData["id"] = id;
            var viewModel = this.requestsService.GetById<SingleRequestViewModel>(id);
            return this.View(viewModel);
        }

        public async Task<IActionResult> Approve(int id)
        {
            await this.requestsService.ApproveById(id);
            return this.Redirect("/Requests/All");
        }

        public async Task<IActionResult> Dismiss(int id)
        {
            await this.requestsService.DismissById(id);
            return this.Redirect("/Requests/All");
        }

        public IActionResult Download(string id)
        {
            var attachment = this.attService.GetFileById(id);
            var rootPath = $"{this.environment.WebRootPath}";
            var pathToFile = rootPath + "/att/requests/" + attachment.Id + attachment.Extension;
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
            return this.File(fileStream,contetType);
        }
    }

}
