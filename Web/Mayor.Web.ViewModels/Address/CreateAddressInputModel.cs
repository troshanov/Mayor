namespace Mayor.Web.ViewModels.Address
{
    using System.ComponentModel.DataAnnotations;

    public class CreateAddressInputModel
    {
        [Required]
        [MinLength(2)]
        [MaxLength(25)]
        public string Street { get; set; }

        [Range(1, 999)]
        public int Number { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string City { get; set; }

        public string PostalCode { get; set; }
    }
}
