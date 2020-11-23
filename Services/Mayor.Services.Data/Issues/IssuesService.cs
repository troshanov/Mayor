using Mayor.Data.Common.Repositories;
using Mayor.Data.Models;
using Mayor.Services.Data.Addresses;
using Mayor.Services.Data.IssueTags;
using Mayor.Web.ViewModels.Issue;
using System.Threading.Tasks;

namespace Mayor.Services.Data.Issues
{
    public class IssuesService : IIssuesService
    {
        private readonly IDeletableEntityRepository<Issue> issuesRepo;
        private readonly IAddressesService addressesService;
        private readonly IIssueTagsService issueTagsService;

        public IssuesService(
            IDeletableEntityRepository<Issue> issuesRepo,
            IAddressesService addressesService,
            IIssueTagsService issueTagsService)
        {
            this.issuesRepo = issuesRepo;
            this.addressesService = addressesService;
            this.issueTagsService = issueTagsService;
        }

        public async Task CreateAsync(CreateIssueInputModel input)
        {
            var address = await this.addressesService.CreateAsync(input.Address);
            var issue = new Issue
            {
                Title = input.Title,
                Description = input.Description,
                Address = address,
                CategoryId = input.CategoryId,
                StatusId = 2,
                CreatorId = 5,
            };

            await this.issuesRepo.AddAsync(issue);
            await this.issuesRepo.SaveChangesAsync();
            await this.issueTagsService.CraeteAsync(issue.Id, input.Tags);
        }
    }
}
