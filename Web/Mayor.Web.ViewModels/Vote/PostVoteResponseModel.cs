namespace Mayor.Web.ViewModels.Vote
{
    using Mayor.Services.Mapping;

    public class PostVoteResponseModel : IMapFrom<Mayor.Data.Models.Issue>
    {
        public int VotesCount { get; set; }
    }
}
