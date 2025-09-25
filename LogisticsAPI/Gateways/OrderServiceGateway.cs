using LogisticsAPI.Gateways.Interfaces;
using LogisticsAPI.Models.DTOs.External.OrderService;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using LogisticsAPI.Models.Results;

namespace LogisticsAPI.Gateways
{
    public class OrderServiceGateway : IOrderServiceGateway
    {
        private readonly HttpClient _httpClient;

        public OrderServiceGateway(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GatewayResult> UpdateOrderStatusAsync(Guid orderId, OrderStatusPatchDTO dto)
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
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new GatewayResult
                {
                    Success = true,
                    StatusCode = (int)response.StatusCode,
                    Message = "Order status updated successfully"
                };
            }

            // Try to extract error details from JSON
            string? details = null;
            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("errors", out var errors))
                {
                    details = errors.ToString();
                }
                else if (doc.RootElement.TryGetProperty("error", out var error))
                {
                    details = error.GetString();
                }
            }
            catch
            {
                details = responseBody; // fallback to raw body
            }

            return new GatewayResult
            {
                Success = false,
                StatusCode = (int)response.StatusCode,
                Message = $"Request failed with {response.StatusCode}",
                Details = details
            };
        }

    }
}