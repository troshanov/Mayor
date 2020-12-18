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

        public int IssuesSubmittedCount { get; set; }

        public int TotalVotesCount { get; set; }

        public int TotalCommentsCount { get; set; }

        // Institution properties
        public string Name { get; set; }

        public string WebsiteUrl { get; set; }

        public string InstitutionType { get; set; }

        public decimal? Rating { get; set; }

        public int SolvedIsseusCount { get; set; }

        public int ActiveIssuesCount { get; set; }
    }
}
