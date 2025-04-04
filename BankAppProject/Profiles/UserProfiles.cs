using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;
using Services.Enums;

namespace BankAppProject.Profiles
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            //CreateMap<UserDto, UserViewModel>()
            //    .ReverseMap();


            CreateMap<UserDto, UserViewModel>()
                .ForMember(dest => dest.Role, opt =>
                opt.MapFrom(src => Enum.Parse<Role>(src.Role, true))); // OBS: inte TryParse

            CreateMap<UserViewModel, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        }
        //private static TEnum ParseEnumOrDefault<TEnum>(string? value, TEnum defaultValue) where TEnum : struct
        //{
        //    return Enum.TryParse<TEnum>(value, true, out var result) ? result : defaultValue;
        //}
    }
}
