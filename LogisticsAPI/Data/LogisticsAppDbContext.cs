using Microsoft.EntityFrameworkCore;
using LogisticsAPI.Models.Entities;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace LogisticsAPI.Data
{
    public class LogisticsAppDbContext : DbContext
    {
        public LogisticsAppDbContext(DbContextOptions<LogisticsAppDbContext> options) : base(options) { }

        public DbSet<Shipment> Shipments => Set<Shipment>();

    }
}
