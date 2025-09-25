using AutoMapper;
using LogisticsAPI.Models.DTOs;
using LogisticsAPI.Models.DTOs.Shipping;
using LogisticsAPI.Models.ValueObjects;

namespace LogisticsAPI.Mappers
{
    public class AvailabilityProfile : Profile
    {
        public AvailabilityProfile()
        {
            CreateMap<Address, AvailabilityResponseDTO>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Valid, opt => opt.MapFrom(_ => true));
        }
    }
}