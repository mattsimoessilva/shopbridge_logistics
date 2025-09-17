using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.Entities
{
    public class Shipment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required Guid OrderId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Status { get; set; }

        public DateOnly? DispatchDate { get; set; }
        public DateOnly? ExpectedArrival { get; set; }

        [Required]
        [MaxLength(100)]
        public required string TrackingCode { get; set; }

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

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}