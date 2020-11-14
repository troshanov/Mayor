namespace Mayor.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Mayor.Data.Common.Models;

    public class Picture : BaseDeletableModel<string>
    {
        public Picture()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Issues = new HashSet<Issue>();
            this.Users = new HashSet<ApplicationUser>();
        }

        public string Extension { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public ICollection<Issue> Issues { get; set; }
    }
}
