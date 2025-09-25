using LogisticsAPI.Models.DTOs.External.OrderService;

namespace LogisticsAPI.Gateways.Interfaces
{
    public interface IOrderServiceGateway
    {
        Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatusPatchDTO dto);
    }
}