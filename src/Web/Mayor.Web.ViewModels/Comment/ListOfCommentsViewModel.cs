using System.Collections.Generic;

namespace Mayor.Web.ViewModels.Comment
{
    public class ListOfCommentsViewModel
    {
        public IEnumerable<CommentInListViewModel> Comments { get; set; }
    }
}
