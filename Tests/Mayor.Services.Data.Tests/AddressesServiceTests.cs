namespace Mayor.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Addresses;
    using Mayor.Services.Data.Cities;
    using Mayor.Web.ViewModels.Address;
    using Moq;
    using Xunit;

    public class AddressesServiceTests
    {
        private List<Address> addressList;
        private Mock<IDeletableEntityRepository<Address>> addressesRepo;
        private List<City> citiesList;
        private Mock<IRepository<City>> citiesRepo;
        private CitiesService citiesService;
        private AddressesService addressesService;

        public AddressesServiceTests()
        {
            this.addressList = new List<Address>();
            this.addressesRepo = new Mock<IDeletableEntityRepository<Address>>();
            this.addressesRepo.Setup(x => x.All()).Returns(this.addressList.AsQueryable());
            this.addressesRepo.Setup(x => x.AddAsync(It.IsAny<Address>())).Callback((Address address) => this.addressList.Add(address));

            this.citiesList = new List<City>();
            this.citiesRepo = new Mock<IRepository<City>>();
            this.citiesRepo.Setup(x => x.All()).Returns(this.citiesList.AsQueryable());
            this.citiesRepo.Setup(x => x.AddAsync(It.IsAny<City>())).Callback((City city) => this.citiesList.Add(city));

            this.citiesService = new CitiesService(this.citiesRepo.Object);
            this.addressesService = new AddressesService(this.addressesRepo.Object, this.citiesService);
        }

        [Fact]
        public async Task AddressShouldBeAddedToDbUponCreation()
        {
            var inputModel = new CreateAddressInputModel
            {
                City = "Test City",
                Number = 1,
                PostalCode = "6000",
                Street = "Test street",
            };

            await this.addressesService.CreateAsync(inputModel);

            Assert.True(this.addressList.Count == 1);
        }

        [Fact]
        public async Task AddressesPropertiesShouldBeMappedCorrectlyUponCreation()
        {
            var inputModel = new CreateAddressInputModel
            {
                City = "Test City",
                Number = 1,
                PostalCode = "6000",
                Street = "Test street",
            };

            var address = await this.addressesService.CreateAsync(inputModel);

            var city = "Test City";
            var number = 1;
            var postalCode = "6000";
            var street = "Test street";

            Assert.Equal(city, address.City.Name);
            Assert.Equal(number, address.StreetNumber);
            Assert.Equal(postalCode, address.PostalCode);
            Assert.Equal(street, address.Street);
        }

        [Fact]
        public async Task AddressShouldNotBeCreatedIfItIsAlreadyContainedInDb()
        {
            var firstModel = new CreateAddressInputModel
            {
                City = "Test City",
                Number = 1,
                PostalCode = "6000",
                Street = "Test street",
            };

            var secondModel = new CreateAddressInputModel
            {
                City = "Test City",
                Number = 1,
                PostalCode = "6000",
                Street = "Test street",
            };

            await this.addressesService.CreateAsync(firstModel);
            await this.addressesService.CreateAsync(secondModel);

            Assert.True(this.addressesRepo.Object.All().Count() == 1);
        }
    }
}
