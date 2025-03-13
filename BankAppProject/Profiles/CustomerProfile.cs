using AutoMapper;
using BankAppProject.ViewModels;
using Services.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankAppProject.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomersDto, CustomersViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname));
        }
    }

}
