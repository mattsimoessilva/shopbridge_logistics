using AutoMapper;
using LogisticsAPI.Models.Entities;
using LogisticsAPI.Models.DTOs.Shipment;

namespace LogisticsAPI.Models.Profiles
{
    public class ShipmentProfile : Profile
    {
        public ShipmentProfile()
        {
            CreateMap<Shipment, ShipmentReadDTO>();
            CreateMap<ShipmentCreateDTO, Shipment>();
            CreateMap<ShipmentUpdateDTO, Shipment>();
            CreateMap<Shipment, Shipment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
