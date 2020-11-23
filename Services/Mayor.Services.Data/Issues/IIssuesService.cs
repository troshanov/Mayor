namespace Mayor.Services.Data.Issues
{
    using System.Threading.Tasks;

    using Mayor.Web.ViewModels.Issue;

    public interface IIssuesService
    {
        Task CreateAsync(CreateIssueInputModel input);
    }
}
