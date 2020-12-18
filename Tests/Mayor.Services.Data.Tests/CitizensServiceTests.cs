namespace Mayor.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Citizens;
    using Mayor.Web.ViewModels.User;
    using Moq;
    using Xunit;

    public class CitizensServiceTests
    {
        private List<Citizen> citizensList;
        private Mock<IDeletableEntityRepository<Citizen>> citizensRepo;
        private CitizensService citizensService;

        public CitizensServiceTests()
        {
            this.citizensList = new List<Citizen>();
            this.citizensRepo = new Mock<IDeletableEntityRepository<Citizen>>();
            this.citizensRepo.Setup(x => x.All()).Returns(this.citizensList.AsQueryable());
            this.citizensRepo.Setup(x => x.AddAsync(It.IsAny<Citizen>())).Callback((Citizen citizen) => this.citizensList.Add(citizen));

            this.citizensService = new CitizensService(this.citizensRepo.Object);
        }

        [Fact]
        public async Task CreateAsyncShouldAddCitizenToDb()
        {
            var userId = "testId";
            var inputModel = new AppUserInputModel
            {
                Birthdate = DateTime.Today,
                Sex = true,
                FirstName = "Test Name",
                LastName = "Test Surname",
            };

            await this.citizensService.CreateAsync(inputModel, userId);

            Assert.True(this.citizensList.Count() == 1);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateCitizenWithCorrectData()
        {
            var userId = "testId";
            var inputModel = new AppUserInputModel
            {
                Birthdate = DateTime.Today,
                Sex = true,
                FirstName = "Test Name",
                LastName = "Test Surname",
            };

            await this.citizensService.CreateAsync(inputModel, userId);
            var citizen = this.citizensList.First();

            var birthday = DateTime.Today.ToUniversalTime();
            var sex = true;
            var firstName = "Test Name";
            var lastName = "Test Surname";

            Assert.Equal(birthday, citizen.Birthdate);
            Assert.Equal(firstName, citizen.FirstName);
            Assert.Equal(lastName, citizen.LastName);
            Assert.Equal(sex, citizen.Sex);
        }

        [Fact]
        public async Task GetByIdShouldReturnCorrectCitizen()
        {
            var userId = "testId";
            var inputModel = new AppUserInputModel
            {
                Birthdate = DateTime.Today,
                Sex = true,
                FirstName = "Test Name",
                LastName = "Test Surname",
            };

            await this.citizensService.CreateAsync(inputModel, userId);
            var citizen = this.citizensList.First();
            citizen.Id = 1;
            this.citizensService.GetById(1);

            Assert.Equal(inputModel.FirstName, citizen.FirstName);
            Assert.Equal(inputModel.LastName, citizen.LastName);
        }

        [Fact]
        public async Task GetByUserIdShouldReturnCorrectCitizen()
        {
            var userId = "testId";
            var inputModel = new AppUserInputModel
            {
                Birthdate = DateTime.Today,
                Sex = true,
                FirstName = "Test Name",
                LastName = "Test Surname",
            };

            await this.citizensService.CreateAsync(inputModel, userId);
            var citizen = this.citizensService.GetByUserId(userId);

            Assert.Equal(inputModel.FirstName, citizen.FirstName);
            Assert.Equal(inputModel.LastName, citizen.LastName);
        }
    }
}
