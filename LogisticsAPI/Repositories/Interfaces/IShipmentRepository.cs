using LogisticsAPI.Models;
using LogisticsAPI.Models.Entities;

namespace LogisticsAPI.Repositories.Interfaces
{
    public interface IShipmentRepository
    {
        Task<Shipment> AddAsync(Shipment productReview);
        Task<IEnumerable<Shipment>> GetAllAsync();
        Task<Shipment?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Shipment productReview);
        Task<bool> DeleteAsync(Guid id);
    }
}