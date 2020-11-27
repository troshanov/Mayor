namespace Mayor.Web.ViewModels.Issue
{
    using System.Collections.Generic;

    public class IssueListViewModel : PagingViewModel
    {
        public IEnumerable<IssueInListViewModel> Issues { get; set; }

    }
}
