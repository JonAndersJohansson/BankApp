using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;

namespace BankAppProject.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserViewModel>()
                .ReverseMap();

            //CreateMap<DataAccessLayer.DTO.UserDto, ViewModels.UserViewModel>()
            //    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Roles.FirstOrDefault()));
        }
    }
}
