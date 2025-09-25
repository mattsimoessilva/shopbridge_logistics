using AutoMapper;
using LogisticsAPI.Gateways.Interfaces;
using LogisticsAPI.Models.DTOs.Shipping;
using LogisticsAPI.Models.Inputs;
using LogisticsAPI.Services.Interfaces;

namespace LogisticsAPI.Services
{
    public class ShippingService : IShippingService
    {
        private readonly IAddressValidationGateway _gateway;
        private readonly IMapper _mapper;

        public ShippingService(IAddressValidationGateway gateway, IMapper mapper)
        {
            _gateway = gateway;
            _mapper = mapper;
        }

        public async Task<AvailabilityResponseDTO> CheckAvailabilityAsync(AvailabilityRequestDTO dto)
        {
            if (dto == null)
                throw new ArgumentException("Invalid request data.", nameof(dto));

            var input = new AddressInput
            {
                Street = dto.Street,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Country = dto.Country
            };

            var normalized = await _gateway.ValidateAndNormalizeAsync(input);

            if (normalized == null)
            {
                return new AvailabilityResponseDTO
                {
                    Valid = false,
                    PostalCode = dto.PostalCode,
                    Country = dto.Country
                };
            }

            bool matches =
                string.Equals(dto.City, normalized.City, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(dto.State, normalized.State, StringComparison.OrdinalIgnoreCase);

            var response = _mapper.Map<AvailabilityResponseDTO>(normalized);
            response.Valid = matches;

            return response;
        }
    }
}