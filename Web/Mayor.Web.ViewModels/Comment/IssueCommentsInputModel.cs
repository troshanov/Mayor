using Microsoft.AspNetCore.Mvc;

namespace Mayor.Web.ViewModels.Comment
{
    public class IssueCommentsInputModel
    {
        [FromBody]
        public int Page { get; set; }

        [FromBody]
        public int IssueId { get; set; }
    }
}
