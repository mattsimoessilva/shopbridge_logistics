using LogisticsAPI.Models.Enums.Shipment;

namespace LogisticsAPI.Utils
{
    public static class ShipmentStatusUtil
    {
        private static readonly Dictionary<ShipmentStatus, HashSet<ShipmentStatus>> AllowedTransitions =
            new()
            {
                { ShipmentStatus.Processing,  new HashSet<ShipmentStatus> { ShipmentStatus.InTransit, ShipmentStatus.Cancelled } },
                { ShipmentStatus.InTransit,   new HashSet<ShipmentStatus> { ShipmentStatus.Completed, ShipmentStatus.Cancelled } },
                { ShipmentStatus.Completed,   new HashSet<ShipmentStatus>() },
                { ShipmentStatus.Cancelled,   new HashSet<ShipmentStatus>() }
            };

        public static bool IsValidTransition(ShipmentStatus current, ShipmentStatus next) =>
            AllowedTransitions.TryGetValue(current, out var validNextStates) &&
            validNextStates.Contains(next);

        public static string? MapToOrderStatus(ShipmentStatus shipmentStatus) =>
            shipmentStatus switch
            {
                ShipmentStatus.InTransit => "InTransit",
                ShipmentStatus.Completed => "Completed",
                ShipmentStatus.Cancelled => "Cancelled",
                _ => null
            };
    }
}