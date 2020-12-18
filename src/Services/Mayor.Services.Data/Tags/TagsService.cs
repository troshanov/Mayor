namespace Mayor.Services.Data.Tags
{
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;

    public class TagsService : ITagsService
    {
        private readonly IRepository<Tag> tagsRepo;

        public TagsService(IRepository<Tag> tagsRepo)
        {
            this.tagsRepo = tagsRepo;
        }

        public async Task<Tag> CreateAsync(string value)
        {
            var tag = this.tagsRepo
                .All()
                .FirstOrDefault(t => t.Value == value);

            if (tag != null)
            {
                return tag;
            }

            tag = new Tag
            {
                Value = value,
            };

            await this.tagsRepo.AddAsync(tag);
            await this.tagsRepo.SaveChangesAsync();

            return tag;
        }
    }
}
