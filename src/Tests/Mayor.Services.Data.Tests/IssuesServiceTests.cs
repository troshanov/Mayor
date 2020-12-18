namespace Mayor.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Addresses;
    using Mayor.Services.Data.Cities;
    using Mayor.Services.Data.Citizens;
    using Mayor.Services.Data.Issues;
    using Mayor.Services.Data.IssueTags;
    using Mayor.Services.Data.Pictures;
    using Mayor.Services.Data.Tags;
    using Mayor.Web.ViewModels.Issue;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class IssuesServiceTests
    {
        private Mock<IFormFile> mockFile;
        private List<Picture> picturesList;
        private Mock<IDeletableEntityRepository<Picture>> picturesRepo;
        private PicturesService picturesService;
        private List<Attachment> attachmentsList;
        private Mock<IDeletableEntityRepository<Attachment>> attachmentsRepo;
        private List<IssueAttachment> issueAttachmentsList;
        private Mock<IRepository<IssueAttachment>> issueAttachmentsRepo;
        private List<Issue> issuesList;
        private Mock<IDeletableEntityRepository<Issue>> issuesRepo;
        private IssuesService issuesService;
        private List<IssueTag> issueTagsList;
        private Mock<IRepository<IssueTag>> issueTagsRepo;
        private IssueTagsService issueTagsService;
        private List<Tag> tagsList;
        private Mock<IRepository<Tag>> tagsRepo;
        private TagsService tagsService;
        private List<Address> addressList;
        private Mock<IDeletableEntityRepository<Address>> addressesRepo;
        private List<City> citiesList;
        private Mock<IRepository<City>> citiesRepo;
        private CitiesService citiesService;
        private AddressesService addressesService;
        private List<Citizen> citizensList;
        private Mock<IDeletableEntityRepository<Citizen>> citizensRepo;
        private CitizensService citizensService;

        public IssuesServiceTests()
        {
            this.mockFile = new Mock<IFormFile>();
            var content = "Hello from my fake file";
            var fileName = "Pic.jpg";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            this.mockFile.Setup(_ => _.OpenReadStream()).Returns(ms);
            this.mockFile.Setup(_ => _.FileName).Returns(fileName);
            this.mockFile.Setup(_ => _.Length).Returns(ms.Length);

            this.picturesList = new List<Picture>();
            this.picturesRepo = new Mock<IDeletableEntityRepository<Picture>>();
            this.picturesRepo.Setup(x => x.All()).Returns(this.picturesList.AsQueryable());
            this.picturesRepo.Setup(x => x.AddAsync(It.IsAny<Picture>())).Callback((Picture picture) => this.picturesList.Add(picture));

            this.picturesService = new PicturesService(this.picturesRepo.Object);

            this.attachmentsList = new List<Attachment>();
            this.attachmentsRepo = new Mock<IDeletableEntityRepository<Attachment>>();
            this.attachmentsRepo.Setup(x => x.All()).Returns(this.attachmentsList.AsQueryable());
            this.attachmentsRepo.Setup(x => x.AddAsync(It.IsAny<Attachment>())).Callback((Attachment att) => this.attachmentsList.Add(att));

            this.issueAttachmentsList = new List<IssueAttachment>();
            this.issueAttachmentsRepo = new Mock<IRepository<IssueAttachment>>();
            this.issueAttachmentsRepo.Setup(x => x.All()).Returns(this.issueAttachmentsList.AsQueryable());
            this.issueAttachmentsRepo.Setup(x => x.AddAsync(It.IsAny<IssueAttachment>())).Callback((IssueAttachment issueAtt) => this.issueAttachmentsList.Add(issueAtt));

            this.issuesList = new List<Issue>();
            this.issuesRepo = new Mock<IDeletableEntityRepository<Issue>>();
            this.issuesRepo.Setup(x => x.All()).Returns(this.issuesList.AsQueryable());
            this.issuesRepo.Setup(x => x.AllAsNoTracking()).Returns(this.issuesList.AsQueryable());
            this.issuesRepo.Setup(x => x.AddAsync(It.IsAny<Issue>())).Callback((Issue issue) => this.issuesList.Add(issue));

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

            this.citizensList = new List<Citizen>();
            this.citizensRepo = new Mock<IDeletableEntityRepository<Citizen>>();
            this.citizensRepo.Setup(x => x.All()).Returns(this.citizensList.AsQueryable());
            this.citizensRepo.Setup(x => x.AllAsNoTracking()).Returns(this.citizensList.AsQueryable());
            this.citizensRepo.Setup(x => x.AddAsync(It.IsAny<Citizen>())).Callback((Citizen citizen) => this.citizensList.Add(citizen));

            this.citizensService = new CitizensService(this.citizensRepo.Object);

            this.citiesList = new List<City>();
            this.citiesRepo = new Mock<IRepository<City>>();
            this.citiesRepo.Setup(x => x.All()).Returns(this.citiesList.AsQueryable());
            this.citiesRepo.Setup(x => x.AddAsync(It.IsAny<City>())).Callback((City city) => this.citiesList.Add(city));

            this.citiesService = new CitiesService(this.citiesRepo.Object);

            this.addressList = new List<Address>();
            this.addressesRepo = new Mock<IDeletableEntityRepository<Address>>();
            this.addressesRepo.Setup(x => x.All()).Returns(this.addressList.AsQueryable());
            this.addressesRepo.Setup(x => x.AddAsync(It.IsAny<Address>())).Callback((Address address) => this.addressList.Add(address));

            this.addressesService = new AddressesService(this.addressesRepo.Object, this.citiesService);

            this.issuesService = new IssuesService(
                this.issuesRepo.Object,
                this.issueAttachmentsRepo.Object,
                this.attachmentsRepo.Object,
                this.citizensRepo.Object,
                this.citizensService,
                this.picturesService,
                this.addressesService,
                this.issueTagsService);
        }

        [Fact]
        public async Task CreateAsyncShouldAddIssueToDb()
        {
            var userId = "someId";
            this.citizensList.Add(new Citizen { Id = 1, UserId = userId });

            var inputModel = new CreateIssueInputModel
            {
                Title = "Test Title",
                Address = new Web.ViewModels.Address.CreateAddressInputModel { City = "Test", Number = 1, Street = "Test" },
                Description = "TestTestTestTestTestTestTest",
                Tags = "uaba daba du",
                CategoryId = 2,
                TitlePicture = this.mockFile.Object,
                Attachments = new List<IFormFile>(),
            };

            var roothPath = "wwwroot";

            await this.issuesService.CreateAsync(inputModel, userId, roothPath);

            Assert.True(this.issuesList.Count() == 1);
        }

        [Fact]
        public async Task GetCountShouldReturnCorrectCount()
        {
            var userId = "someId";
            this.citizensList.Add(new Citizen { Id = 1, UserId = userId });

            var inputModel = new CreateIssueInputModel
            {
                Title = "Test Title",
                Address = new Web.ViewModels.Address.CreateAddressInputModel { City = "Test", Number = 1, Street = "Test" },
                Description = "TestTestTestTestTestTestTest",
                Tags = "uaba daba du",
                CategoryId = 2,
                TitlePicture = this.mockFile.Object,
                Attachments = new List<IFormFile>(),
            };

            var roothPath = "wwwroot";

            await this.issuesService.CreateAsync(inputModel, userId, roothPath);
            var issueCount = this.issuesService.GetCount();

            Assert.Equal(this.issuesList.Count(), issueCount);
        }

        [Fact]
        public async Task GetCountByCategoryShouldReturnCorrectCount()
        {
            var userId = "someId";
            this.citizensList.Add(new Citizen { Id = 1, UserId = userId });

            var inputModel = new CreateIssueInputModel
            {
                Title = "Test Title",
                Address = new Web.ViewModels.Address.CreateAddressInputModel { City = "Test", Number = 1, Street = "Test" },
                Description = "TestTestTestTestTestTestTest",
                Tags = "uaba daba du",
                CategoryId = 2,
                TitlePicture = this.mockFile.Object,
                Attachments = new List<IFormFile>(),
            };

            var roothPath = "wwwroot";

            await this.issuesService.CreateAsync(inputModel, userId, roothPath);
            inputModel.CategoryId = 3;
            await this.issuesService.CreateAsync(inputModel, userId, roothPath);
            await this.issuesService.CreateAsync(inputModel, userId, roothPath);
            foreach (var issue in this.issuesList)
            {
                if (issue.CategoryId == 2)
                {
                    issue.Category = new Category { Name = "Second" };
                    continue;
                }

                issue.Category = new Category { Name = "First" };
            }

            var issueCount = this.issuesService.GetCountByCateogry("Second");

            Assert.Equal(1, issueCount);
        }

        [Fact]
        public async Task GetCountByUserIdShouldReturnCorrectCount()
        {
            var userId = "someId";
            this.citizensList.Add(new Citizen { Id = 1, UserId = "anotherId" });
            this.citizensList.Add(new Citizen { Id = 2, UserId = "someId" });

            var inputModel = new CreateIssueInputModel
            {
                Title = "Test Title",
                Address = new Web.ViewModels.Address.CreateAddressInputModel { City = "Test", Number = 1, Street = "Test" },
                Description = "TestTestTestTestTestTestTest",
                Tags = "uaba daba du",
                CategoryId = 2,
                TitlePicture = this.mockFile.Object,
                Attachments = new List<IFormFile>(),
            };

            var roothPath = "wwwroot";

            await this.issuesService.CreateAsync(inputModel, userId, roothPath);
            await this.issuesService.CreateAsync(inputModel, userId, roothPath);
            userId = "anotherId";
            await this.issuesService.CreateAsync(inputModel, userId, roothPath);

            var issuesCount = this.issuesService.GetCountByUserId(userId);

            Assert.True(issuesCount == 1);
        }

    }
}
