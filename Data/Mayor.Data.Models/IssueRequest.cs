namespace Mayor.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Mayor.Data.Common.Models;

    public class IssueRequest : BaseDeletableModel<int>
    {
        public IssueRequest()
        {
            this.IssueRequestAttachments = new HashSet<IssueRequestAttachment>();
        }

        public int RequesterId { get; set; }

        public virtual Institution Requester { get; set; }

        public int IssueId { get; set; }

        public virtual Issue Issue { get; set; }

        public DateTime DateSubmitted { get; set; }

        public string Description { get; set; }

        public bool IsSolveRequest { get; set; }

        public bool IsAcknowledged { get; set; }

        public bool IsApproved { get; set; }

        public ICollection<IssueRequestAttachment> IssueRequestAttachments { get; set; }
    }
}
