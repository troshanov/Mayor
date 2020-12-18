namespace Mayor.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Mayor.Data.Common.Models;

    public class Address : BaseDeletableModel<int>
    {
        public Address()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Issues = new HashSet<Issue>();
        }

        [Required]
        [MaxLength(25)]
        public string Street { get; set; }

        public int StreetNumber { get; set; }

        public string PostalCode { get; set; }

        public int CityId { get; set; }

        public virtual City City { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public ICollection<Issue> Issues { get; set; }
    }
}
