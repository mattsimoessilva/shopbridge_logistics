
namespace LogisticsAPI.Models.Results
{
    public class GatewayResult
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Details { get; set; }
    }
}
