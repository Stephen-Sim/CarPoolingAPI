using AutoMapper;
using CarPoolingAPI.Dto;
using CarPoolingAPI.Models;

namespace CarPoolingAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Vehicle, VehicleDto>();
            CreateMap<VehicleDto, Vehicle>();
        } 
    }
}
