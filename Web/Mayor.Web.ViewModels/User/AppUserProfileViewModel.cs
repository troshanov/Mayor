using System.ComponentModel.DataAnnotations;

namespace Mayor.Web.ViewModels.User
{
    public class AppUserProfileViewModel
    {
        public bool IsCitizen { get; set; }

        public bool IsProfileOwner { get; set; }

        public string PhoneNumber { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public string ProfilePicUrl { get; set; }

        // Citizen properties
        public string FullName { get; set; }

        public string Birthdate { get; set; }

        public string Sex { get; set; }

        // Institution properties
        public string Name { get; set; }

        public string WebsiteUrl { get; set; }

        public string InstitutionType { get; set; }

        public decimal? Rating { get; set; }
    }
}
