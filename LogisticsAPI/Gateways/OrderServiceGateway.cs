using LogisticsAPI.Gateways.Interfaces;
using LogisticsAPI.Models.DTOs.External.OrderService;
using System.Net.Http;
using System.Net.Http.Json;

namespace LogisticsAPI.Gateways
{
    public class OrderServiceGateway : IOrderServiceGateway
    {
        private readonly HttpClient _httpClient;

        public OrderServiceGateway(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatusPatchDTO dto)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Invalid order ID", nameof(orderId));

            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status cannot be null or empty.", nameof(dto.Status));

            var payload = new OrderStatusPatchDTO { Status = dto.Status };

            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/orders/{orderId}/status")
            {
                Content = JsonContent.Create(payload)
            };

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}