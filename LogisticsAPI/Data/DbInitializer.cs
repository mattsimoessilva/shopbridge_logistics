using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LogisticsAPI.Data;
using LogisticsAPI.Models.Entities;

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
                new Shipment { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Pending", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)), TrackingCode = "TRK123456789", Carrier = "FedEx", ServiceLevel = "Express", Street = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", Country = "USA", CreatedAt = DateTime.UtcNow },
                new Shipment { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Shipped", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)), TrackingCode = "TRK987654321", Carrier = "UPS", ServiceLevel = "Standard", Street = "456 Oak Ave", City = "Chicago", State = "IL", PostalCode = "60601", Country = "USA", CreatedAt = DateTime.UtcNow },
                new Shipment { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Delivered", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), TrackingCode = "TRK555666777", Carrier = "DHL", ServiceLevel = "Overnight", Street = "789 Pine Rd", City = "Los Angeles", State = "CA", PostalCode = "90001", Country = "USA", CreatedAt = DateTime.UtcNow }
            };

            context.Shipments.AddRange(entities);
            context.SaveChanges();
        }
    }
}