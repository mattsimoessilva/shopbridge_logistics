using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.DTOs.Shipment
{
    public class ShipmentCreateDTO
    {
        [Required]
        public required Guid OrderId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Status { get; set; } 

        public DateOnly? DispatchDate { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Carrier { get; set; }

        [Required]
        [MaxLength(30)]
        public required string ServiceLevel { get; set; } 

        [Required]
        [MaxLength(100)]
        public required string Street { get; set; }

        [Required]
        [MaxLength(50)]
        public required string City { get; set; }

        [Required]
        [MaxLength(50)]
        public required string State { get; set; }

        [Required]
        [MaxLength(20)]
        public required string PostalCode { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Country { get; set; }
    }
}