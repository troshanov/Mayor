using Mayor.Data.Common.Models;

namespace Mayor.Data.Models
{
    public class Vote
    {
        public int CitizenId { get; set; }

        public Citizen Citizen { get; set; }

        public int IssueId { get; set; }

        public Issue Issue { get; set; }
    }
}
