namespace Mayor.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Reviews;
    using Moq;
    using Xunit;

    public class ReviewsServiceTests
    {
        private List<IssueReview> reviewsList;
        private Mock<IRepository<IssueReview>> reviewsRepo;
        private ReviewsService reviewsService;

        public ReviewsServiceTests()
        {
            this.reviewsList = new List<IssueReview>();
            this.reviewsRepo = new Mock<IRepository<IssueReview>>();
            this.reviewsRepo.Setup(x => x.All()).Returns(this.reviewsList.AsQueryable());
            this.reviewsRepo.Setup(x => x.AllAsNoTracking()).Returns(this.reviewsList.AsQueryable());
            this.reviewsRepo.Setup(x => x.AddAsync(It.IsAny<IssueReview>())).Callback((IssueReview review) => this.reviewsList.Add(review));

            this.reviewsService = new ReviewsService(this.reviewsRepo.Object);
        }

        [Fact]
        public async Task CreateAsyncShouldAddReviewToDb()
        {
            var citizenId = 1;
            var issueId = 1;
            var score = 5;
            var comment = "Test";

            await this.reviewsService.CreateAsync(citizenId, issueId, score, comment);

            Assert.True(this.reviewsList.Count() == 1);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateIssueReviewWithCorrectValues()
        {
            var citizenId = 1;
            var issueId = 1;
            var score = 5;
            var comment = "Test";

            await this.reviewsService.CreateAsync(citizenId, issueId, score, comment);
            var issue = this.reviewsList.First();

            Assert.Equal(citizenId, issue.CitizenId);
            Assert.Equal(issueId, issue.IssueId);
            Assert.Equal(score, issue.Score);
            Assert.Equal(comment, issue.Comment);
        }

        [Fact]
        public async Task HasReviewedIssueShouldReturnCorrectValue()
        {
            var citizenId = 1;
            var issueId = 1;
            var score = 5;
            var comment = "Test";

            await this.reviewsService.CreateAsync(citizenId, issueId, score, comment);
            var hasReviewedIssue = this.reviewsService.HasReviewedIssue(citizenId, issueId);

            Assert.True(hasReviewedIssue);
        }
    }
}
