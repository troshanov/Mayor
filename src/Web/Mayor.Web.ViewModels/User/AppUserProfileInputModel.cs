using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mayor.Web.ViewModels.User
{
    public class AppUserProfileInputModel : IValidatableObject
    {
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePicture { get; set; }

        public string Description { get; set; }

        public bool IsCitizen { get; set; }

        // Citizen properties
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public DateTime? Birthdate { get; set; }

        public bool? Sex { get; set; }

        // Institution properties
        public string Name { get; set; }

        public string Website { get; set; }

        [Display(Name = "Institution Type")]
        public bool IsGovernment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.IsCitizen == true)
            {
                if (!new RequiredAttribute().IsValid(this.FirstName))
                {
                    yield return new ValidationResult("First Name is required.", new[] { nameof(this.FirstName) });
                }
                else
                {
                    if (this.FirstName.Length > 15)
                    {
                        yield return new ValidationResult("First Name lenght should not exceed 15 characters.", new[] { nameof(this.FirstName) });
                    }

                    if (this.FirstName.Length < 2)
                    {
                        yield return new ValidationResult("First Name should contain at least 2 characters.", new[] { nameof(this.FirstName) });
                    }
                }

                if (!new RequiredAttribute().IsValid(this.LastName))
                {
                    yield return new ValidationResult("Last Name is required.", new[] { nameof(this.LastName) });
                }
                else
                {
                    if (this.LastName.Length > 20)
                    {
                        yield return new ValidationResult("Last Name lenght should not exceed 15 characters.", new[] { nameof(this.LastName) });
                    }

                    if (this.LastName.Length < 2)
                    {
                        yield return new ValidationResult("Last Name should contain at least 2 characters.", new[] { nameof(this.LastName) });
                    }
                }

                if (this.Birthdate.HasValue)
                {
                    if (this.Birthdate.Value.Year < 1900 ||
                        this.Birthdate.Value.Year > DateTime.UtcNow.Year)
                    {
                        yield return new ValidationResult($"Birthdate's year should be between 1900 and {DateTime.UtcNow.Year}.", new[] { nameof(this.Birthdate) });
                    }
                }

                if (!this.Sex.HasValue)
                {
                    yield return new ValidationResult("Sex field is required.", new[] { nameof(this.Sex) });
                }
            }
            else
            {
                if (!new RequiredAttribute().IsValid(this.Name))
                {
                    yield return new ValidationResult("Name is required.", new[] { nameof(this.Name) });
                }
                else
                {
                    if (this.Name.Length > 50)
                    {
                        yield return new ValidationResult("Name lenght should not exceed 50 characters.", new[] { nameof(this.Name) });
                    }

                    if (this.Name.Length < 2)
                    {
                        yield return new ValidationResult("Name should contain at least 2 characters.", new[] { nameof(this.Name) });
                    }
                }

                if (new RequiredAttribute().IsValid(this.Website))
                {
                    if (!new UrlAttribute().IsValid(this.Website))
                    {
                        yield return new ValidationResult("Invalid website URL.", new[] { nameof(this.Website) });
                    }
                }

                if (!new RequiredAttribute().IsValid(this.IsGovernment))
                {
                    yield return new ValidationResult("Institution Type filed is required.", new[] { nameof(this.IsGovernment) });
                }
            }
        }
    }
}
