using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models.DTOs.Shipment
{
    public class ShipmentStatusUpdateDTO
    {

        [MaxLength(50)]
        public string? Status { get; set; } = string.Empty;
    }
}