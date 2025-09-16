using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace LogisticsAPI.Models.Entities
{
    public class Shipment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required Guid OrderId { get; set; }

        [Required]
        public required Guid CustomerId {get;set;}

        [Required]
        [MaxLength(50)]
        public required string Status { get; set; }

        public DateOnly DispatchDate { get; set; }

        public DateOnly? ExpectedArtival { get; set; }

        public required string TrackingCode { get; set; }

        [MaxLength(50)]
        public required string Carrrier { get; set; }

        [MaxLength(30)]
        public required string ServiceLevel { get; set; }

        [Required]
        public required Guid DestinationAdressId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
