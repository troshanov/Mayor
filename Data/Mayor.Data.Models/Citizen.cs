namespace Mayor.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Mayor.Data.Common.Models;

    public class Citizen : BaseDeletableModel<int>
    {
        public Citizen()
        {
            this.Issues = new HashSet<Issue>();
            this.IssueReviews = new HashSet<IssueReview>();
        }

        [Required]
        [MaxLength(15)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        public DateTime? Birthdate { get; set; }

        public bool? Sex { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public ICollection<Issue> Issues { get; set; }

        public ICollection<IssueReview> IssueReviews { get; set; }
    }
}
