using LogisticsAPI.Models.DTOs.External.OrderService;
using LogisticsAPI.Models.Results;

namespace LogisticsAPI.Gateways.Interfaces
{
    public interface IOrderServiceGateway
    {
        Task<GatewayResult> UpdateOrderStatusAsync(Guid orderId, OrderStatusPatchDTO dto);
    }
}