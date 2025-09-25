using AutoMapper;
using LogisticsAPI.Gateways.Interfaces;
using LogisticsAPI.Models;
using LogisticsAPI.Models.DTOs.External.OrderService;
using LogisticsAPI.Models.DTOs.Shipment;
using LogisticsAPI.Models.Entities;
using LogisticsAPI.Models.Enums.Shipment;
using LogisticsAPI.Repositories.Interfaces;
using LogisticsAPI.Services.Interfaces;
using LogisticsAPI.Utils;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Linq;

namespace LogisticsAPI.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _repository;
        private readonly IOrderServiceGateway _gateway;
        private readonly IMapper _mapper;

        public ShipmentService(IShipmentRepository repository, IOrderServiceGateway gateway, IMapper mapper)
        {
            _repository = repository;
            _gateway = gateway;
            _mapper = mapper;
        }

        public async Task<ShipmentReadDTO> CreateAsync(ShipmentCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<Shipment>(dto);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.Status = ShipmentStatus.Processing;
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

            if (!ShipmentStatusUtil.IsValidTransition(existing.Status, patch))
                throw new InvalidOperationException($"Invalid transition {existing.Status} → {patch}");

            var orderStatus = ShipmentStatusUtil.MapToOrderStatus(patch);

            if (orderStatus != null)
            {
                var result = await _gateway.UpdateOrderStatusAsync(
                    existing.OrderId,
                    new OrderStatusPatchDTO { Status = orderStatus }
                );

                if (!result.Success)
                {
                    var message = $"Failed to update Order {existing.OrderId} status to {orderStatus}. " +
                                  $"Shipment not updated. Reason: {result.Message}";

                    if (!string.IsNullOrWhiteSpace(result.Details))
                        message += $" Details: {result.Details}";

                    throw new InvalidOperationException(message);
                }
            }

            existing.Status = patch;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);

            return true;
        }
    }
}