using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;

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
