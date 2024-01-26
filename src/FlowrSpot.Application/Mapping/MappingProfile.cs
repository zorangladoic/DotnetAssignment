using AutoMapper;
using FlowrSpot.Domain.Entities;
using FlowrSpot.Dtos;

namespace FlowrSpot.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Flower, FlowerDto>().ReverseMap();
            CreateMap<CreateFlowerRequest, Flower>();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<RegisterUserRequest, User>();
            CreateMap<Sighting, SightingDto>().ReverseMap();
            CreateMap<CreateSightingRequest, Sighting>();
            CreateMap<Like, LikeDto>().ReverseMap();
            CreateMap<CreateLikeRequest, Like>();
        }
    }
}
