namespace Mayor.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Mayor.Data.Common.Models;

    public class Tag : BaseModel<int>
    {
        public Tag()
        {
            this.IssueTags = new HashSet<IssueTag>();
        }

        [Required]
        public string Value { get; set; }

        public virtual ICollection<IssueTag> IssueTags { get; set; }
    }
}
