using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.DTOs.Address
{
    public class AddressCreateDTO
    {
        [Required]
        public required Guid CustomerId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Street { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Neighborhood { get; set; }

        [Required]
        [MaxLength(50)]
        public required string City { get; set; }

        [Required]
        [MaxLength(2)]
        public required string State { get; set; }

        [Required]
        [MaxLength(10)]
        public required string PostalCode { get; set; }

        [MaxLength(100)]
        public string? Complement { get; set; }
    }
}
