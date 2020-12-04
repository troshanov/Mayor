using Mayor.Services.Data.Institutions;
using Mayor.Web.ViewModels.Institution;
using Microsoft.AspNetCore.Mvc;

namespace Mayor.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IInstitutionsService institutionsService;

        public UsersController(IInstitutionsService institutionsService)
        {
            this.institutionsService = institutionsService;
        }

        public IActionResult Top()
        {
            var vewModel = new TopInstitutionsListViewModel
            {
                Institutions = this.institutionsService.GetTopTenByRating<InstitutionViewModel>(),
            };
            return View(vewModel);
        }
    }
}
