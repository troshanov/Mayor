namespace Mayor.Services.Data.Addresses
{
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Cities;
    using Mayor.Web.ViewModels.Address;

    public class AddressesService : IAddressesService
    {
        private readonly IDeletableEntityRepository<Address> addressRepo;
        private readonly ICitiesService citiesService;

        public AddressesService(
            IDeletableEntityRepository<Address> addressRepo,
            ICitiesService citiesService)
        {
            this.addressRepo = addressRepo;
            this.citiesService = citiesService;
        }

        public async Task<Address> CreateAsync(CreateAddressInputModel input)
        {
            var city = await this.citiesService.CreateAsync(input.City);
            var address = this.addressRepo.All()
                .FirstOrDefault(a => a.Street == input.Street
                                    && a.StreetNumber == input.Number
                                    && a.City.Name == city.Name);

            if (address != null)
            {
                return address;
            }

            address = new Address
            {
                Street = input.Street,
                StreetNumber = input.Number,
                PostalCode = input.PostalCode,
                City = city,
            };

            await this.addressRepo.AddAsync(address);
            await this.addressRepo.SaveChangesAsync();

            return address;
        }
    }
}
