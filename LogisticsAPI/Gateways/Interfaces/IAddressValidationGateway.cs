using LogisticsAPI.Models.Inputs;
using LogisticsAPI.Models.ValueObjects;

namespace LogisticsAPI.Gateways.Interfaces
{
    public interface IAddressValidationGateway
    {
        Task<Address?> ValidateAndNormalizeAsync(AddressInput input);
    }
}