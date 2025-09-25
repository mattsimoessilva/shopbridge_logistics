using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.DTOs.Shipping
{
    public class AvailabilityRequestDTO
    {
        [Required]
        [StringLength(20)]
        public required string PostalCode { get; set; }

        [Required]
        [StringLength(50)]
        public required string Country { get; set; } = "BR";

        [Required]
        [StringLength(100)]
        public required string Street { get; set; }

        [Required]
        [StringLength(50)]
        public required string City { get; set; }

        [Required]
        [StringLength(50)]
        public required string State { get; set; }
    }
}
