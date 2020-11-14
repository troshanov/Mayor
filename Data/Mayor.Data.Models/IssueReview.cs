namespace Mayor.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class IssueReview
    {
        public int CitizenId { get; set; }

        public virtual Citizen Citizen { get; set; }

        public int IssueId { get; set; }

        public virtual Issue Issue { get; set; }

        public int Score { get; set; }

        public string Comment { get; set; }
    }
}
