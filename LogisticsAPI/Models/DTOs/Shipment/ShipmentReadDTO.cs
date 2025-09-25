using LogisticsAPI.Models.Enums.Shipment;
using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.DTOs.Shipment
{
    public class ShipmentReadDTO
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public required ShipmentStatus Status { get; set; }

        public DateOnly? DispatchDate { get; set; }

        public DateOnly? ExpectedArrival { get; set; }

        public required string TrackingCode { get; set; }

        public required string Carrier { get; set; }

        public required string ServiceLevel { get; set; }

        public required string Street { get; set; }

        public required string City { get; set; }

        public required string State { get; set; }

        public required string PostalCode { get; set; }

        public required string Country { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}