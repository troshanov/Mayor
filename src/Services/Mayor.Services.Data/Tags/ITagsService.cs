namespace Mayor.Services.Data.Tags
{
    using System.Threading.Tasks;

    using Mayor.Data.Models;

    public interface ITagsService
    {
        Task<Tag> CreateAsync(string value);
    }
}
