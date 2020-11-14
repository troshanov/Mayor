namespace Mayor.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class IssueTag
    {
        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }

        public int IssueId { get; set; }

        public virtual Issue Issue { get; set; }
    }
}
