using AutoMapper;
using LogisticsAPI.Models.Entities;
using LogisticsAPI.Models.DTOs.Address;

namespace LogisticsAPI.Models.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressReadDTO>();
            CreateMap<AddressCreateDTO, Address>();
            CreateMap<AddressUpdateDTO, Address>();
            CreateMap<Address, Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
