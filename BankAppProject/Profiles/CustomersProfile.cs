using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Profiles
{
    public class CustomersProfile : Profile
    {
        public CustomersProfile()
        {
            CreateMap<CustomersDto, CustomersViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname));
        }
    }

}
