namespace Mayor.Services.Data.IssueTags
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Tags;

    public class IssueTagsService : IIssueTagsService
    {
        private readonly IRepository<IssueTag> repository;
        private readonly IStringOperationsService stringService;
        private readonly ITagsService tagsService;

        public IssueTagsService(
            IRepository<IssueTag> repository,
            IStringOperationsService stringService,
            ITagsService tagsService)
        {
            this.repository = repository;
            this.stringService = stringService;
            this.tagsService = tagsService;
        }

        public async Task CraeteAsync(int issueId, string tagsString)
        {
            if (string.IsNullOrWhiteSpace(tagsString))
            {
                return;
            }

            var parsedTags = this.stringService
                .SplitByEmptySpace(tagsString.ToLower());

            var uniqueTags = new HashSet<string>(parsedTags);

            foreach (var uniqueTag in uniqueTags)
            {
                var tag = await this.tagsService.CreateAsync(uniqueTag);
                var issueTag = new IssueTag
                {
                    IssueId = issueId,
                    Tag = tag,
                };

                await this.repository.AddAsync(issueTag);
            }

            await this.repository.SaveChangesAsync();
        }
    }
}
