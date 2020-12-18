namespace Mayor.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Mayor.Data.Common.Models;

    public class Attachment : BaseDeletableModel<string>
    {
        public Attachment()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IssueAttachments = new HashSet<IssueAttachment>();
            this.IssueRequestAttachments = new HashSet<IssueRequestAttachment>();
        }

        [Required]
        public string AddedByUserId { get; set; }

        [Required]
        public string Extension { get; set; }

        public ApplicationUser AddedByUser { get; set; }

        public ICollection<IssueAttachment> IssueAttachments { get; set; }

        public ICollection<IssueRequestAttachment> IssueRequestAttachments { get; set; }
    }
}
