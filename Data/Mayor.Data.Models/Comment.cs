namespace Mayor.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Mayor.Data.Common.Models;

    public class Comment : BaseModel<int>
    {
        public int IssueId { get; set; }

        public Issue Issue { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        [MaxLength(160)]
        public string Content { get; set; }
    }
}
