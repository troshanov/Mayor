namespace Mayor.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Citizens;
    using Mayor.Services.Data.Comments;
    using Mayor.Services.Data.Institutions;
    using Mayor.Web.ViewModels.Comment;
    using Moq;
    using Xunit;

    public class CommentsServiceTests
    {
        private List<Comment> commentsList;
        private Mock<IRepository<Comment>> commentsRepo;
        private CommentsService commentsService;
        private List<Citizen> citizensList;
        private Mock<IDeletableEntityRepository<Citizen>> citizensRepo;
        private CitizensService citizensService;
        private List<Institution> institutionsList;
        private Mock<IDeletableEntityRepository<Institution>> institutionsRepo;
        private InstitutionsService institutionsService;
        private List<IssueReview> reviewsList;
        private Mock<IRepository<IssueReview>> reviewsRepo;
        private List<Issue> issuesList;
        private Mock<IDeletableEntityRepository<Issue>> issuesRepo;

        public CommentsServiceTests()
        {
            this.commentsList = new List<Comment>();
            this.commentsRepo = new Mock<IRepository<Comment>>();
            this.commentsRepo.Setup(x => x.All()).Returns(this.commentsList.AsQueryable());
            this.commentsRepo.Setup(x => x.AddAsync(It.IsAny<Comment>())).Callback((Comment comment) => this.commentsList.Add(comment));

            this.citizensList = new List<Citizen>();
            this.citizensRepo = new Mock<IDeletableEntityRepository<Citizen>>();
            this.citizensRepo.Setup(x => x.All()).Returns(this.citizensList.AsQueryable());
            this.citizensRepo.Setup(x => x.AddAsync(It.IsAny<Citizen>())).Callback((Citizen citizen) => this.citizensList.Add(citizen));

            this.citizensService = new CitizensService(this.citizensRepo.Object);

            this.reviewsList = new List<IssueReview>();
            this.reviewsRepo = new Mock<IRepository<IssueReview>>();
            this.reviewsRepo.Setup(x => x.All()).Returns(this.reviewsList.AsQueryable());
            this.reviewsRepo.Setup(x => x.AllAsNoTracking()).Returns(this.reviewsList.AsQueryable());
            this.reviewsRepo.Setup(x => x.AddAsync(It.IsAny<IssueReview>())).Callback((IssueReview review) => this.reviewsList.Add(review));

            this.issuesList = new List<Issue>();
            this.issuesRepo = new Mock<IDeletableEntityRepository<Issue>>();
            this.issuesRepo.Setup(x => x.All()).Returns(this.issuesList.AsQueryable());
            this.issuesRepo.Setup(x => x.AddAsync(It.IsAny<Issue>())).Callback((Issue issue) => this.issuesList.Add(issue));

            this.institutionsList = new List<Institution>();
            this.institutionsRepo = new Mock<IDeletableEntityRepository<Institution>>();
            this.institutionsRepo.Setup(x => x.All()).Returns(this.institutionsList.AsQueryable());
            this.institutionsRepo.Setup(x => x.AddAsync(It.IsAny<Institution>())).Callback((Institution institution) => this.institutionsList.Add(institution));

            this.institutionsService = new InstitutionsService(this.institutionsRepo.Object, this.reviewsRepo.Object, this.issuesRepo.Object);
            this.commentsService = new CommentsService(this.commentsRepo.Object, this.citizensService, this.institutionsService);
        }

        [Fact]
        public async Task CreateAsyncShouldAddCommentToDb()
        {
            var inputModel = new CommentInputModel
            {
                Content = "Test Content",
                IssueId = 1,
                UserId = "testUser",
            };

            await this.commentsService.CreateAsync(inputModel);

            Assert.True(this.commentsList.Count() == 1);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateCommentWithCorrectValues()
        {
            var inputModel = new CommentInputModel
            {
                Content = "Test Content",
                IssueId = 1,
                UserId = "testUser",
            };

            await this.commentsService.CreateAsync(inputModel);
            var comment = this.commentsList.First();

            Assert.Equal(inputModel.Content, comment.Content);
            Assert.Equal(inputModel.IssueId, comment.IssueId);
            Assert.Equal(inputModel.UserId, comment.UserId);
        }

        [Fact]
        public async Task GetAllByIssueIdShouldReturnCorrectComments()
        {
            this.institutionsList.Add(new Institution { UserId = "testUser", Name = "Test Name" });

            var inputModel = new CommentInputModel
            {
                Content = "Test Content",
                IssueId = 1,
                UserId = "testUser",
            };
            await this.commentsService.CreateAsync(inputModel);

            inputModel.IssueId = 2;
            await this.commentsService.CreateAsync(inputModel);

            inputModel.IssueId = 1;
            await this.commentsService.CreateAsync(inputModel);

            foreach (var comment in this.commentsList)
            {
                comment.User = new ApplicationUser();
                comment.CreatedOn = DateTime.Today;
            }

            var comments = this.commentsService.GetAllByIssueId(1, 1);

            Assert.True(comments.Count() == 2);
        }

        [Fact]
        public async Task GetCountByIssueIdShouldReturnCorrectCount()
        {
            var inputModel = new CommentInputModel
            {
                Content = "Test Content",
                IssueId = 1,
                UserId = "testUser",
            };
            await this.commentsService.CreateAsync(inputModel);

            inputModel.IssueId = 2;
            await this.commentsService.CreateAsync(inputModel);

            inputModel.IssueId = 2;
            await this.commentsService.CreateAsync(inputModel);

            var commentsCount = this.commentsService.GetCountByIssueId(1);

            Assert.True(commentsCount == 1);
        }

    }
}
