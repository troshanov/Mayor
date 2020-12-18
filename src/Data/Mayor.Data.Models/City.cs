namespace Mayor.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Mayor.Data.Common.Models;

    public class City : BaseModel<int>
    {
        public City()
        {
            this.Addresses = new HashSet<Address>();
        }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public ICollection<Address> Addresses { get; set; }
    }
}
