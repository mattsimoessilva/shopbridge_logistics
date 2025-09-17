using LogisticsAPI.Models.DTOs.Shipment;
using LogisticsAPI.Models.Entities;

namespace LogisticsAPI.Services.Interfaces
{
    public interface IShipmentService
    {
        public Task<ShipmentReadDTO> CreateAsync(ShipmentCreateDTO dto);
        public Task<IEnumerable<ShipmentReadDTO>> GetAllAsync();
        public Task<ShipmentReadDTO?> GetByIdAsync(Guid id);
        public Task<bool> UpdateAsync(ShipmentUpdateDTO dto);
        public Task<bool> DeleteAsync(Guid id);
    }
}
