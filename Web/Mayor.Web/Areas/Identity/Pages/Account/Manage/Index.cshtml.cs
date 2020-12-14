namespace Mayor.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Citizens;
    using Mayor.Services.Data.Institutions;
    using Mayor.Services.Data.Pictures;
    using Mayor.Web.ViewModels.User;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment environment;
        private readonly ICitizensService citizensService;
        private readonly IInstitutionsService institutionsService;
        private readonly IPicturesService picService;
        private readonly IDeletableEntityRepository<Picture> picRepo;
        private readonly IDeletableEntityRepository<Citizen> citizenRepo;
        private readonly IDeletableEntityRepository<Institution> institutionRepo;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment environment,
            ICitizensService citizensService,
            IInstitutionsService institutionsService,
            IPicturesService picService,
            IDeletableEntityRepository<Picture> picRepo,
            IDeletableEntityRepository<Citizen> citizenRepo,
            IDeletableEntityRepository<Institution> institutionRepo)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this.environment = environment;
            this.citizensService = citizensService;
            this.institutionsService = institutionsService;
            this.picService = picService;
            this.picRepo = picRepo;
            this.citizenRepo = citizenRepo;
            this.institutionRepo = institutionRepo;
        }

        public string Username { get; set; }

        public string PicId { get; set; }

        public string PicExtension { get; set; }

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
            var profilePic = this.picService.GetProfilePicByUserId(user.Id);
            var description = this._userManager.FindByIdAsync(user.Id).Result.Description;

            this.Username = userName;
            this.PicId = profilePic.Id;
            this.PicExtension = profilePic.Extension;

            this.Input = new AppUserProfileInputModel
            {
                PhoneNumber = phoneNumber,
                Description = description,
            };

            if (this.User.IsInRole("Citizen"))
            {
                var citizen = this.citizensService.GetByUserId(user.Id);

                this.Input.FirstName = citizen.FirstName;
                this.Input.LastName = citizen.LastName;
                this.Input.Birthdate = citizen.Birthdate;
                this.Input.Sex = citizen.Sex;
                this.Input.IsCitizen = true;
            }
            else if (this.User.IsInRole("Institution"))
            {
                var institution = this.institutionsService.GetByUserId(user.Id);

                this.Input.Name = institution.Name;
                this.Input.Website = institution.WebsiteUrl;
                this.Input.IsGovernment = institution.IsGovernment;
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

            var profilePic = this.picService.GetProfilePicByUserId(user.Id);
            this.PicId = profilePic.Id;
            this.PicExtension = profilePic.Extension;

            if (this.Input.ProfilePicture != null)
            {
                try
                {
                    var newProfilePic = await this.picService.CreateFileAsync(user.Id, $"{this.environment.WebRootPath}", this.Input.ProfilePicture);
                    await this.picService.DeleteOldProfilePicAsync(user.Id);
                    await this.picRepo.AddAsync(newProfilePic);
                    await this.picRepo.SaveChangesAsync();

                    this.PicId = newProfilePic.Id;
                    this.PicExtension = newProfilePic.Extension;
                }
                catch (Exception ex)
                {
                    this.ModelState.AddModelError(string.Empty, ex.Message);
                    return this.Page();
                }
            }

            var description = user.Description;
            if (this.Input.Description != description)
            {
                user.Description = this.Input.Description;
                var setDescriptionResult = await this._userManager.UpdateAsync(user);
                if (!setDescriptionResult.Succeeded)
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
