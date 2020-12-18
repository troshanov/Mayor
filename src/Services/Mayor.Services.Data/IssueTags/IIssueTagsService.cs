namespace Mayor.Services.Data.IssueTags
{
    using System.Threading.Tasks;

    public interface IIssueTagsService
    {
        Task CraeteAsync(int issueId, string tagsString);
    }
}
