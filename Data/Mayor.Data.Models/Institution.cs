namespace Mayor.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Mayor.Data.Common.Models;

    public class Institution : BaseDeletableModel<int>
    {
        public Institution()
        {
            this.IssueRequests = new HashSet<IssueRequest>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public decimal Rating { get; set; }

        public bool IsGovernment { get; set; }

        [MaxLength(500)]
        public string WebsiteUrl { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public ICollection<IssueRequest> IssueRequests { get; set; }

        public ICollection<Issue> Issues { get; set; }
    }
}
