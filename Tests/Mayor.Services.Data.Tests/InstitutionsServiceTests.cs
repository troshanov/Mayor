namespace Mayor.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Institutions;
    using Mayor.Web.ViewModels.User;
    using Moq;
    using Xunit;

    public class InstitutionsServiceTests
    {
        private List<IssueReview> reviewsList;
        private Mock<IRepository<IssueReview>> reviewsRepo;
        private List<Issue> issuesList;
        private Mock<IDeletableEntityRepository<Issue>> issuesRepo;
        private List<Institution> institutionsList;
        private Mock<IDeletableEntityRepository<Institution>> institutionsRepo;
        private InstitutionsService institutionsService;

        public InstitutionsServiceTests()
        {
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
        }

        [Fact]
        public async Task CreateAsyncShouldAddInstitutionToDb()
        {
            var userId = "testId";
            var inputModel = new AppUserInputModel
            {
                Name = "Test Name",
                Website = "Test Site",
                IsGovernment = true,
            };

            await this.institutionsService.CreateAsync(inputModel, userId);

            Assert.True(this.institutionsList.Count() == 1);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateInstitutionWithCorrectData()
        {
            var userId = "testId";
            var inputModel = new AppUserInputModel
            {
                Name = "Test Name",
                Website = "Test Site",
                IsGovernment = true,
            };

            await this.institutionsService.CreateAsync(inputModel, userId);
            var institution = this.institutionsList.First();

            Assert.Equal(inputModel.Name, institution.Name);
            Assert.Equal(inputModel.Website, institution.WebsiteUrl);
            Assert.Equal(inputModel.IsGovernment, institution.IsGovernment);
        }

        [Fact]
        public async Task GetByIdShouldReturnCorrectInstitution()
        {
            var userId = "testId";
            var inputModel = new AppUserInputModel
            {
                Name = "Test Name",
                Website = "Test Site",
                IsGovernment = true,
            };
            await this.institutionsService.CreateAsync(inputModel, userId);
            this.institutionsList.First().Id = 1;

            inputModel.Name = "Test Test";
            await this.institutionsService.CreateAsync(inputModel, userId);

            var institution = this.institutionsService.GetById(1);

            Assert.Equal("Test Name", institution.Name);
        }

        [Fact]
        public async Task GetByUserIdShouldReturnCorrectInstitution()
        {
            var userId = "testId";
            var inputModel = new AppUserInputModel
            {
                Name = "Test Name",
                Website = "Test Site",
                IsGovernment = true,
            };
            await this.institutionsService.CreateAsync(inputModel, userId);

            userId = "anotherId";
            await this.institutionsService.CreateAsync(inputModel, userId);

            var institution = this.institutionsService.GetByUserId(userId);

            Assert.Equal(institution.UserId, userId);
        }
    }
}
