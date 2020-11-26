using Mayor.Data.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mayor.Services.Data.Citizens;
using Mayor.Data.Common.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mayor.Services.Data.Institutions;
using Mayor.Web.ViewModels.User;

namespace Mayor.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICitizensService citizensService;
        private readonly IInstitutionsService institutionsService;
        private readonly IDeletableEntityRepository<Citizen> citizenRepo;
        private readonly IDeletableEntityRepository<Institution> institutionRepo;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICitizensService citizensService,
            IInstitutionsService institutionsService,
            IDeletableEntityRepository<Citizen> citizenRepo,
            IDeletableEntityRepository<Institution> institutionRepo)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this.citizensService = citizensService;
            this.institutionsService = institutionsService;
            this.citizenRepo = citizenRepo;
            this.institutionRepo = institutionRepo;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public AppUserProfileInputModel Input { get; set; }

        //public class InputModel
        //{
        //    [Phone]
        //    [Display(Name = "Phone number")]
        //    public string PhoneNumber { get; set; }

        //    public bool IsCitizen { get; set; }

        //    // Citizen properties
        //    [Required]
        //    [MinLength(2)]
        //    [MaxLength(15)]
        //    [Display(Name = "First Name")]
        //    public string FirstName { get; set; }

        //    [Required]
        //    [MinLength(2)]
        //    [MaxLength(20)]
        //    [Display(Name = "Last Name")]
        //    public string LastName { get; set; }

        //    [ValidYearAttribute]
        //    public DateTime? Birthdate { get; set; }

        //    public bool? Sex { get; set; }

        //    // Institution properties
        //    [Required]
        //    [MinLength(2)]
        //    [MaxLength(50)]
        //    public string Name { get; set; }

        //    [Url]
        //    public string Website { get; set; }

        //    public bool IsGovernment { get; set; }
        //}

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await this._userManager.GetUserNameAsync(user);
            var phoneNumber = await this._userManager.GetPhoneNumberAsync(user);

            this.Username = userName;

            if (this.User.IsInRole("Citizen"))
            {
                var citizen = this.citizensService.GetByUserId(user.Id);

                this.Input = new AppUserProfileInputModel
                {
                    PhoneNumber = phoneNumber,
                    FirstName = citizen.FirstName,
                    LastName = citizen.LastName,
                    Birthdate = citizen.Birthdate,
                    Sex = citizen.Sex,
                    IsCitizen = true,
                };
            }
            else if (this.User.IsInRole("Institution"))
            {
                var institution = this.institutionsService.GetByUserId(user.Id);

                this.Input = new AppUserProfileInputModel
                {
                    PhoneNumber = phoneNumber,
                    Name = institution.Name,
                    Website = institution.WebsiteUrl,
                    IsGovernment = institution.IsGovernment,
                    IsCitizen = false,
                };
            }
            else
            {
                this.Input = new AppUserProfileInputModel
                {
                    PhoneNumber = phoneNumber,
                };
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var userName = await this._userManager.GetUserNameAsync(user);
            this.Username = userName;

            var phoneNumber = await this._userManager.GetPhoneNumberAsync(user);
            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this._userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    this.StatusMessage = "Unexpected error when trying to set phone number.";
                    return this.RedirectToPage();
                }
            }

            if (this.User.IsInRole("Citizen"))
            {
                var citizen = this.citizensService.GetByUserId(user.Id);

                var firstName = citizen.FirstName;
                if (firstName != this.Input.FirstName)
                {
                    citizen.FirstName = this.Input.FirstName;
                }

                var lastName = citizen.LastName;
                if (lastName != this.Input.LastName)
                {
                    citizen.LastName = this.Input.LastName;
                }

                var birthdate = citizen.Birthdate;
                if (birthdate?.ToUniversalTime() != this.Input.Birthdate?.ToUniversalTime())
                {
                    citizen.Birthdate = this.Input.Birthdate;
                }

                var sex = citizen.Sex;
                if (sex != this.Input.Sex)
                {
                    citizen.Sex = this.Input.Sex;
                }

                await this.citizenRepo.SaveChangesAsync();
            }
            else if (this.User.IsInRole("Institution"))
            {
                var institution = this.institutionsService.GetByUserId(user.Id);

                var name = institution.Name;
                if (name != this.Input.Name)
                {
                    institution.Name = this.Input.Name;
                }

                var websiteUrl = institution.WebsiteUrl;
                if (websiteUrl != this.Input.Website)
                {
                    institution.WebsiteUrl = this.Input.Website;
                }

                await this.institutionRepo.SaveChangesAsync();
            }

            await this._signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.Page();
        }
    }
}
