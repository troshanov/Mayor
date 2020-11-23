namespace Mayor.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Mayor.Data.Common.Models;

    public class Picture : BaseDeletableModel<string>
    {
        public Picture()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Extension { get; set; }

        [Required]
        public string AddedByUserId { get; set; }

        public ApplicationUser AddedByUser { get; set; }

        public int IssueId { get; set; }

        public Issue Issue { get; set; }
    }
}
