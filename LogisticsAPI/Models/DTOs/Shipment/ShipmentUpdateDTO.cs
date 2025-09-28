using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.DTOs.Shipment
{
    public class ShipmentUpdateDTO
    {

        public DateOnly? DispatchDate { get; set; }

        public DateOnly? ExpectedArrival { get; set; }

        [MaxLength(100)]
        public string? TrackingCode { get; set; }

        [MaxLength(50)]
        public string? Carrier { get; set; }

        [MaxLength(30)]
        public string? ServiceLevel { get; set; }

        [MaxLength(100)]
        public string? Street { get; set; }

        [MaxLength(50)]
        public string? City { get; set; }

        [MaxLength(50)]
        public string? State { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(50)]
        public string? Country { get; set; }
    }
}