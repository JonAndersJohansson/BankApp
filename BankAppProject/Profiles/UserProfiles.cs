using AutoMapper;
using BankAppProject.ViewModels;
using Services.DTOs;
using Services.Enums;

namespace BankAppProject.Profiles
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<UserDto, UserViewModel>()
                .ForMember(dest => dest.Role, opt =>
                opt.MapFrom(src => Enum.Parse<Role>(src.Role, true)));

            CreateMap<UserViewModel, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        }
    }
}
