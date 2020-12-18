namespace Mayor.Services.Data.Addresses
{
    using System.Threading.Tasks;

    using Mayor.Data.Models;
    using Mayor.Web.ViewModels.Address;

    public interface IAddressesService
    {
        Task<Address> CreateAsync(CreateAddressInputModel input);
    }
}
