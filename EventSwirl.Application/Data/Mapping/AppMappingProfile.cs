using AutoMapper;
using EventSwirl.Application.Data.DTOs;
using EventSwirl.Domain.Entities;

namespace EventSwirl.Application.Data.Mapping
{
    public class AppMappingProfile: Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Event, EventDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
