using AutoMapper;
using LogisticsAPI.Models;
using LogisticsAPI.Models.DTOs.Shipment;
using LogisticsAPI.Models.Entities;
using LogisticsAPI.Models.Enums.Shipment;
using LogisticsAPI.Repositories.Interfaces;
using LogisticsAPI.Services.Interfaces;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Linq;

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
            entity.Status = ShipmentStatus.Pending;
            entity.TrackingCode = $"TRK-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

            await _repository.AddAsync(entity);

            return _mapper.Map<ShipmentReadDTO>(entity);
        }

        public async Task<IEnumerable<ShipmentReadDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<ShipmentReadDTO>();

            return _mapper.Map<IEnumerable<ShipmentReadDTO>>(entities);
        }

        public async Task<ShipmentReadDTO?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID", nameof(id));

            var entity = await _repository.GetByIdAsync(id);

            return entity is null ? null : _mapper.Map<ShipmentReadDTO>(entity);
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

        public async Task<bool> UpdateStatusAsync(Guid id, ShipmentStatusPatchDTO dto)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID", nameof(id));

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return false;

            if (!Enum.TryParse<ShipmentStatus>(dto.Status, true, out var patch))
                throw new ArgumentException($"Invalid status value: {dto.Status}", nameof(dto.Status));

            var allowedTransitions = new Dictionary<ShipmentStatus, HashSet<ShipmentStatus>>
            {
                { ShipmentStatus.Pending,     new HashSet<ShipmentStatus> { ShipmentStatus.Processing, ShipmentStatus.Cancelled } },
                { ShipmentStatus.Processing,  new HashSet<ShipmentStatus> { ShipmentStatus.InTransit, ShipmentStatus.Cancelled } },
                { ShipmentStatus.InTransit,   new HashSet<ShipmentStatus> { ShipmentStatus.Completed, ShipmentStatus.Cancelled } },
                { ShipmentStatus.Completed,   new HashSet<ShipmentStatus>() },
                { ShipmentStatus.Cancelled,   new HashSet<ShipmentStatus>() }
            };

            if (!allowedTransitions.TryGetValue(existing.Status, out var validNextStates)
                || !validNextStates.Contains(patch))
            {
                throw new InvalidOperationException(
                    $"Inappropriate status transition: {existing.Status} → {patch}");
            }

            existing.Status = patch;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);

            return true;
        }
    }
}