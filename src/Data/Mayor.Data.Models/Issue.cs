﻿namespace Mayor.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Mayor.Data.Common.Models;

    public class Issue : BaseDeletableModel<int>
    {
        public Issue()
        {
            this.IssueReviews = new HashSet<IssueReview>();
            this.IssueRequests = new HashSet<IssueRequest>();
            this.IssueTags = new HashSet<IssueTag>();
            this.IssueAttachments = new HashSet<IssueAttachment>();
            this.Pictures = new HashSet<Picture>();
            this.Votes = new HashSet<Vote>();
            this.IssueComments = new HashSet<Comment>();
        }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public int CreatorId { get; set; }

        public virtual Citizen Creator { get; set; }

        public int? SolverId { get; set; }

        public virtual Institution Solver { get; set; }

        public int AddressId { get; set; }

        public virtual Address Address { get; set; }

        public int StatusId { get; set; }

        public virtual Status Status { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public ICollection<Picture> Pictures { get; set; }

        public ICollection<IssueAttachment> IssueAttachments { get; set; }

        public ICollection<IssueTag> IssueTags { get; set; }

        public ICollection<IssueRequest> IssueRequests { get; set; }

        public ICollection<IssueReview> IssueReviews { get; set; }

        public ICollection<Vote> Votes { get; set; }

        public virtual ICollection<Comment> IssueComments { get; set; }
    }
}
