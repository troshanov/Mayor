namespace Mayor.Web.ViewModels.Issue
{
    using System.Collections.Generic;

    public class TopIssuesListViewModel
    {
        public IEnumerable<TopIssueViewModel> Issues { get; set; }
    }
}
