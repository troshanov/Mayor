namespace Mayor.Web.ViewModels.Address
{
    using System.ComponentModel.DataAnnotations;

    public class CreateAddressInputModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Street must containt at least 2 characters.")]
        [MaxLength(25)]
        public string Street { get; set; }

        [Range(1, 999, ErrorMessage = "Number must be between 1 and 999.")]
        public int Number { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "City must containt at least 2 characters.")]
        [MaxLength(20)]
        public string City { get; set; }

        public string PostalCode { get; set; }
    }
}
