using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.DTOs.Shipment
{
    public class ShipmentCreateDTO
    {
        [Required]
        public required Guid OrderId { get; set; }

        [Required]
        public required Guid CustomerId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Status { get; set; }

        public DateOnly DispatchDate { get; set; }

        [MaxLength(50)]
        public required string Carrrier { get; set; }

        [MaxLength(30)]
        public required string ServiceLevel { get; set; }

        [Required]
        public required Guid DestinationAdressId { get; set; }
    }
}
