namespace Mayor.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Cities;
    using Moq;
    using Xunit;

    public class CitiesServiceTests
    {
        private List<City> citiesList;
        private CitiesService citiesService;
        private Mock<IRepository<City>> citiesRepo;

        public CitiesServiceTests()
        {
            this.citiesList = new List<City>();
            this.citiesRepo = new Mock<IRepository<City>>();
            this.citiesRepo.Setup(x => x.All()).Returns(this.citiesList.AsQueryable());
            this.citiesRepo.Setup(x => x.AddAsync(It.IsAny<City>())).Callback((City city) => this.citiesList.Add(city));

            this.citiesService = new CitiesService(this.citiesRepo.Object);
        }

        [Fact]
        public async Task CityShouldBeAddedToDbUponCreation()
        {
            await this.citiesService.CreateAsync("Test City");

            Assert.True(this.citiesList.Count() == 1);
        }

        [Fact]
        public async Task CityShouldNotBeAddedIfAlreadyContainedInDb()
        {
            await this.citiesService.CreateAsync("Test City");
            await this.citiesService.CreateAsync("Test City");

            Assert.True(this.citiesList.Count() == 1);
        }

        [Fact]
        public async Task CityPropertiesShouldBeMappedCorrectly()
        {
            var city = await this.citiesService.CreateAsync("Test City");
            var expectedName = "Test City";

            Assert.True(city.Name == expectedName);
        }
    }
}
