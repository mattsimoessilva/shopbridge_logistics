using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.DTOs.Shipment
{
    public class ShipmentStatusPatchDTO
    {

        [MaxLength(50)]
        public string? Status { get; set; } = string.Empty;
    }
}