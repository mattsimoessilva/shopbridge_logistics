using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.DTOs.Shipment
{
    public class ShipmentUpdateDTO
    {
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        public DateOnly? DispatchDate { get; set; }

        public DateOnly? ExpectedArrival { get; set; }

        [MaxLength(100)]
        public string? TrackingCode { get; set; }

        [MaxLength(50)]
        public string? Carrier { get; set; }

        [MaxLength(30)]
        public string? ServiceLevel { get; set; }

        public Guid? DestinationAddressId { get; set; }
    }
}
