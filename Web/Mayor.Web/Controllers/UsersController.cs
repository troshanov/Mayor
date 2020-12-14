using Mayor.Data.Common.Repositories;
using Mayor.Data.Models;
using Mayor.Services.Data.Citizens;
using Mayor.Services.Data.Institutions;
using Mayor.Web.ViewModels.Institution;
using Mayor.Web.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace Mayor.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IInstitutionsService institutionsService;
        private readonly ICitizensService citizensService;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepo;

        public UsersController(
            IInstitutionsService institutionsService,
            ICitizensService citizensService,
            IDeletableEntityRepository<ApplicationUser> usersRepo)
        {
            this.institutionsService = institutionsService;
            this.citizensService = citizensService;
            this.usersRepo = usersRepo;
        }

        public IActionResult Top()
        {
            var vewModel = new TopInstitutionsListViewModel
            {
                Institutions = this.institutionsService.GetTopTenByRating<InstitutionViewModel>(),
            };
            return View(vewModel);
        }

        public IActionResult Profile(string profileId)
        {
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var citizen = this.citizensService.GetByUserId(profileId);
            var institution = this.institutionsService.GetByUserId(profileId);
            var viewModel = new AppUserProfileViewModel();

            ApplicationUser user;
            if (citizen != null)
            {
                user = this.usersRepo.All()
                .Include(u => u.Pictures)
                .FirstOrDefault(u => u.Id == citizen.UserId);

                viewModel.FullName = citizen.FirstName + " " + citizen.LastName;
                viewModel.Birthdate = citizen.Birthdate == null ? "N/A" : citizen.Birthdate?.ToString("g");
                viewModel.Sex = citizen.Sex == true ? "female" : "male";
            }
            else
            {
                user = this.usersRepo.All()
                .Include(u => u.Pictures)
                .FirstOrDefault(u => u.Id == institution.UserId);

                viewModel.InstitutionType = institution.IsGovernment ? "Government" : "NGO";
                viewModel.Name = institution.Name;
                viewModel.WebsiteUrl = institution.WebsiteUrl ?? "N/A";
                viewModel.Rating = institution.Rating;
            }

            viewModel.IsCitizen = citizen != null;
            viewModel.Description = user.Description == null ? "N/A" : user.Description;
            viewModel.Email = user.Email;
            viewModel.PhoneNumber = user.PhoneNumber == null ? "N/A" : user.PhoneNumber;
            viewModel.ProfilePicUrl = user.Pictures.Any(p => p.IssueId == null) ? "/img/" + user.Pictures
                                    .FirstOrDefault(p => p.IssueId == null).Id +
                                    user.Pictures.FirstOrDefault(p => p.IssueId == null).Extension
                                    : "/img/anon.png";
            viewModel.IsProfileOwner = currentUserId == user.Id;

            return this.View(viewModel);
        }
    }
}
