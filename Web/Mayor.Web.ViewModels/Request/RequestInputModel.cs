namespace Mayor.Web.ViewModels.Request
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class RequestInputModel
    {
        public int IssueId { get; set; }

        [Required]
        public bool IsSolveRequest { get; set; }

        [Required]
        [MinLength(20, ErrorMessage = "Description must containt at least 20 characters.")]
        public string Description { get; set; }

        public ICollection<IFormFile> Attachments { get; set; }
    }
}
