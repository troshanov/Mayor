using Mayor.Web.ViewModels.Request;
using System.ComponentModel.DataAnnotations;

namespace Mayor.Web.ViewModels.Review
{
    public class ReviewInputModel
    {
        public int IssueId { get; set; }

        public SingleRequestViewModel SolveRequest { get; set; }

        [Range(1, 5)]
        public int Score { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Description must containt at least 10 characters.")]
        public string Comment { get; set; }

        public bool HasReviewed { get; set; }
    }
}
