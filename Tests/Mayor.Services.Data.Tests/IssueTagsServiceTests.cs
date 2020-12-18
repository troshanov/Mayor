namespace Mayor.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.IssueTags;
    using Mayor.Services.Data.Tags;
    using Moq;
    using Xunit;

    public class IssueTagsServiceTests
    {
        private List<IssueTag> issueTagsList;
        private Mock<IRepository<IssueTag>> issueTagsRepo;
        private IssueTagsService issueTagsService;
        private List<Tag> tagsList;
        private Mock<IRepository<Tag>> tagsRepo;
        private TagsService tagsService;

        public IssueTagsServiceTests()
        {
            this.issueTagsList = new List<IssueTag>();
            this.issueTagsRepo = new Mock<IRepository<IssueTag>>();
            this.issueTagsRepo.Setup(x => x.All()).Returns(this.issueTagsList.AsQueryable());
            this.issueTagsRepo.Setup(x => x.AddAsync(It.IsAny<IssueTag>())).Callback((IssueTag issueTag) => this.issueTagsList.Add(issueTag));

            this.tagsList = new List<Tag>();
            this.tagsRepo = new Mock<IRepository<Tag>>();
            this.tagsRepo.Setup(x => x.All()).Returns(this.tagsList.AsQueryable());
            this.tagsRepo.Setup(x => x.AddAsync(It.IsAny<Tag>())).Callback((Tag tag) => this.tagsList.Add(tag));

            this.tagsService = new TagsService(this.tagsRepo.Object);
            this.issueTagsService = new IssueTagsService(this.issueTagsRepo.Object, new StringOperationsServices(), this.tagsService);
        }

        [Fact]
        public async Task CreateAsyncShouldAddIssueTagToDb()
        {
            var tagsString = "waba daba du";
            var issueId = 1;

            await this.issueTagsService.CraeteAsync(issueId, tagsString);

            Assert.True(this.issueTagsList.Count() == 3);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateIssueTagsWithCorrectValues()
        {
            var tagsString = "waba daba du";
            var issueId = 1;

            await this.issueTagsService.CraeteAsync(issueId, tagsString);
            var issueTags = this.issueTagsRepo.Object.All().ToArray();
            var firstIssueTag = issueTags[0];
            var secondIssueTag = issueTags[1];
            var thirdIssueTag = issueTags[2];

            Assert.True(firstIssueTag.Tag.Value == "waba" && firstIssueTag.IssueId == 1);
            Assert.True(secondIssueTag.Tag.Value == "daba" && secondIssueTag.IssueId == 1);
            Assert.True(thirdIssueTag.Tag.Value == "du" && thirdIssueTag.IssueId == 1);
        }

        [Fact]
        public async Task CreateAsyncShouldNotCreateATagIfItIsAlreadyContainedInDb()
        {
            var tagsString = "waba daba daba";
            var issueId = 1;

            await this.issueTagsService.CraeteAsync(issueId, tagsString);

            Assert.True(this.issueTagsList.Count() == 2);
        }
    }
}
