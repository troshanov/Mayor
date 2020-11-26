namespace Mayor.Web.ViewModels.Issue
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Mayor.Web.ViewModels.Address;
    using Microsoft.AspNetCore.Http;

    public class CreateIssueInputModel
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must containt at least 5 characters.")]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MinLength(20, ErrorMessage = "Description must containt at least 20 characters.")]
        public string Description { get; set; }

        public CreateAddressInputModel Address { get; set; }

        public string Tags { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public string Test { get; set; }

        [Required]
        public IFormFile TitlePicture { get; set; }

        public ICollection<IFormFile> Attachments { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }
    }
}
