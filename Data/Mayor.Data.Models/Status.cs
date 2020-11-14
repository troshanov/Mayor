namespace Mayor.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Status
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StatusCode { get; set; }
    }
}
