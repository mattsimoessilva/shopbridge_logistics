using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LogisticsAPI.Data;
using LogisticsAPI.Models.Entities;
using LogisticsAPI.Models.Enums.Shipment;

namespace LogisticsAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(LogisticsAppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Shipments.Any())
                return;

            var entities = new[]
            {
                new Shipment { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = ShipmentStatus.Processing, DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)), TrackingCode = "TRK123456789", Carrier = "Correios", ServiceLevel = "Sedex", Street = "Avenida Paulista, 1578", City = "São Paulo", State = "SP", PostalCode = "01310-200", Country = "Brasil", CreatedAt = DateTime.UtcNow },
                new Shipment { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = ShipmentStatus.Completed, DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)), TrackingCode = "TRK987654321", Carrier = "Jadlog", ServiceLevel = "Econômico", Street = "Rua XV de Novembro, 500", City = "Curitiba", State = "PR", PostalCode = "80020-310", Country = "Brasil", CreatedAt = DateTime.UtcNow },
                new Shipment { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = ShipmentStatus.Cancelled, DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), TrackingCode = "TRK555666777", Carrier = "LATAM Cargo", ServiceLevel = "Expresso", Street = "Avenida Atlântica, 1702", City = "Rio de Janeiro", State = "RJ", PostalCode = "22021-001", Country = "Brasil", CreatedAt = DateTime.UtcNow }
            };

            context.Shipments.AddRange(entities);
            context.SaveChanges();
        }
    }
}