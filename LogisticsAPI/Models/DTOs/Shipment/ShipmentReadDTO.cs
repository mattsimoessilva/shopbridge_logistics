using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.DTOs.Shipment
{
    public class ShipmentReadDTO
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid CustomerId { get; set; }

        public required string Status { get; set; }

        public DateOnly DispatchDate { get; set; }

        public DateOnly? ExpectedArrival { get; set; }

        public required string TrackingCode { get; set; }

        public required string Carrier { get; set; }

        public required string ServiceLevel { get; set; }

        public Guid DestinationAddressId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
