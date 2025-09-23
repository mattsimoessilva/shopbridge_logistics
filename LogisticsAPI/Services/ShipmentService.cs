using AutoMapper;
using LogisticsAPI.Models;
using LogisticsAPI.Models.DTOs.Shipment;
using LogisticsAPI.Models.Entities;
using LogisticsAPI.Repositories.Interfaces;
using LogisticsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LogisticsAPI.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _repository;
        private readonly IMapper _mapper;

        public ShipmentService(IShipmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ShipmentReadDTO> CreateAsync(ShipmentCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<Shipment>(dto);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.TrackingCode = $"TRK-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

            await _repository.AddAsync(entity);

            return _mapper.Map<ShipmentReadDTO>(entity);
        }

        public async Task<IEnumerable<ShipmentReadDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<ShipmentReadDTO>();

            var result = _mapper.Map<IEnumerable<ShipmentReadDTO>>(entities);

            return result;
        }

        public async Task<ShipmentReadDTO?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID", nameof(id));

            var entity = await _repository.GetByIdAsync(id);

            if (entity is null)
                return null;

            return _mapper.Map<ShipmentReadDTO>(entity);
        }

        public async Task<bool> UpdateAsync(ShipmentUpdateDTO dto)
        {
            if (dto == null || dto.Id == Guid.Empty)
                throw new ArgumentException("Invalid update data.");

            var existing = await _repository.GetByIdAsync(dto.Id);
            if (existing == null)
                return false;

            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);

            return true;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID", nameof(id));

            await _repository.DeleteAsync(id);

            return true;
        }

        public async Task<bool> UpdateStatusAsync(Guid id, ShipmentStatusUpdateDTO dto)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID", nameof(id));

            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status cannot be null or empty.", nameof(dto.Status));

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return false;

            existing.Status = dto.Status;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);

            return true;
        }

    }
}
