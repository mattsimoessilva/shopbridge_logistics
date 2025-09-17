using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.DTOs.Address
{
    public class AddressReadDTO
    {
        public Guid Id { get; set; }

        public required Guid CustomerId { get; set; }

        public required string Street { get; set; }

        public required string Neighborhood { get; set; }

        public required string City { get; set; }

        public required string State { get; set; }

        public required string PostalCode { get; set; }

        public string? Complement { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
