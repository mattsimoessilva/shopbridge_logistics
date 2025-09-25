using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.Inputs
{
    public class AddressInput
    {
        [Required]
        [MaxLength(200)]
        public string? Street { get; set; }

        [Required]
        [MaxLength(100)]
        public string? City { get; set; }

        [Required]
        [MaxLength(100)]
        public string? State { get; set; }

        [Required]
        [MaxLength(20)]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(2)]
        public string Country { get; set; } = string.Empty;
    }
}