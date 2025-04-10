using AutoMapper;
using DataAccessLayer.Models;
using Services.DTOs;

namespace Services.Infrastructure.Profiles
{
    public class UserServiceProfile : Profile
    {
        public UserServiceProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.Ignore()); // manuellt
        }
    }
}
