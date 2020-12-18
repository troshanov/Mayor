namespace Mayor.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Tags;
    using Moq;
    using Xunit;

    public class TagsServiceTests
    {
        private List<Tag> tagsList;
        private Mock<IRepository<Tag>> tagsRepo;
        private TagsService tagsService;

        public TagsServiceTests()
        {
            this.tagsList = new List<Tag>();
            this.tagsRepo = new Mock<IRepository<Tag>>();
            this.tagsRepo.Setup(x => x.All()).Returns(this.tagsList.AsQueryable());
            this.tagsRepo.Setup(x => x.AddAsync(It.IsAny<Tag>())).Callback((Tag tag) => this.tagsList.Add(tag));

            this.tagsService = new TagsService(this.tagsRepo.Object);
        }

        [Fact]
        public async Task CreateAsyncShouldAddTagToDb()
        {
            var tagValue = "test";

            await this.tagsService.CreateAsync(tagValue);

            Assert.True(this.tagsList.Count() == 1);
        }

        [Fact]
        public async Task CreateAsyncShouldNotCreateTagIfItsValueIsAlreadyContainedInDb()
        {
            var tagValue = "test";
            var secondTagValue = "test";

            await this.tagsService.CreateAsync(tagValue);
            await this.tagsService.CreateAsync(secondTagValue);

            Assert.True(this.tagsList.Count() == 1);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateTagWithCorrectValue()
        {
            var tagValue = "test";
            await this.tagsService.CreateAsync(tagValue);

            var value = this.tagsList.First().Value;
            var expectedValue = tagValue;

            Assert.Equal(expectedValue, value);
        }
    }
}
