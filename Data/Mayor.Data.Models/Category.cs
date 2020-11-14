namespace Mayor.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Mayor.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.Issues = new HashSet<Issue>();
        }

        [Required]
        public string Name { get; set; }

        public ICollection<Issue> Issues { get; set; }
    }
}
