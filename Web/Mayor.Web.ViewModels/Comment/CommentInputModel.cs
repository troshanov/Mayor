namespace Mayor.Web.ViewModels.Comment
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;

    public class CommentInputModel
    {
        [Required]
        [FromForm]
        [MinLength(2)]
        [MaxLength(160)]
        [Display(Name = "Comment")]
        public string Content { get; set; }

        [Required]
        [FromForm]
        public string UserId { get; set; }

        [FromForm]
        public int IssueId { get; set; }
    }
}
