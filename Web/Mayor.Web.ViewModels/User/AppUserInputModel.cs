namespace Mayor.Web.ViewModels.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AppUserInputModel : IValidatableObject
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "User Type")]
        public bool IsCitizen { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public DateTime? Birthdate { get; set; }

        public bool? Sex { get; set; }

        public string Name { get; set; }

        public string Website { get; set; }

        [Display(Name = "Institution Type")]
        public bool? IsGovernment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validate Citizen properties:
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
                    if (this.LastName.Length > 15)
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
