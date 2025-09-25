using LogisticsAPI.Models.DTOs.Shipping;
using LogisticsAPI.Models.Entities;

namespace LogisticsAPI.Services.Interfaces
{
    public interface IShippingService
    {
        public Task<AvailabilityResponseDTO> CheckAvailabilityAsync(AvailabilityRequestDTO dto);
    }
}
