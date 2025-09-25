namespace LogisticsAPI.Models.DTOs.Shipping
{
    public class AvailabilityResponseDTO
    {
        public bool Valid { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Street { get; set; }
    }

}
