using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.DTOs.Address
{
    public class AddressUpdateDTO
    {
        public Guid Id { get; set; }

        public required Guid CustomerId { get; set; }

        [MaxLength(100)]
        public required string Street { get; set; }

        [MaxLength(50)]
        public required string Neighborhood { get; set; }

        [MaxLength(50)]
        public required string City { get; set; }

        [MaxLength(2)]
        public required string State { get; set; }

        [MaxLength(10)]
        public required string PostalCode { get; set; }

        [MaxLength(100)]
        public string? Complement { get; set; }

    }
}
