using AutoMapper;
using Microsoft.EntityFrameworkCore;
using LogisticsAPI.Data;
using LogisticsAPI.Models;
using LogisticsAPI.Models.Entities;
using LogisticsAPI.Repositories.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LogisticsAPI.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly LogisticsAppDbContext _context;
        private readonly IMapper _mapper;
        public ShipmentRepository(LogisticsAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Shipment> AddAsync(Shipment entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Shipments.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<Shipment>> GetAllAsync()
        {
            return await _context.Shipments
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Shipment?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(
                    "Id cannot be an empty GUID",
                    nameof(id));

            return await _context.Shipments
                .AsNoTracking()
                .Include(p => p.DestinationAdress)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> UpdateAsync(Shipment updated)
        {
            if (updated == null)
                throw new ArgumentNullException(nameof(updated));

            var existing = await _context.Shipments.FirstOrDefaultAsync(p => p.Id == updated.Id);
            if (existing == null) return false;

            _mapper.Map(updated, existing);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(
                    "Id cannot be an empty GUID",
                    nameof(id));

            var entity = await _context.Shipments.FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null) return false;

            _context.Shipments.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}