using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;

namespace Services.Profiles
{
    public class CustomerIndexProfile : Profile
    {
        public CustomerIndexProfile()
        {
            CreateMap<CustomerIndexDto, CustomerIndexViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname));
        }
    }

}
