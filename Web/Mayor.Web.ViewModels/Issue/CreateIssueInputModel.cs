﻿namespace Mayor.Web.ViewModels.Issue
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Mayor.Web.ViewModels.Address;

    public class CreateIssueInputModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MinLength(20)]
        public string Description { get; set; }

        public CreateAddressInputModel Address { get; set; }

        public string Tags { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }
    }
}