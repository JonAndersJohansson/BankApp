using AutoMapper;
using DataAccessLayer.Models;
using Services.DTOs;

namespace Services.Infrastructure.Profiles
{
    public class CustomerServiceProfiles : Profile
    {             
        public CustomerServiceProfiles()
        {
            CreateMap<Customer, CustomerIndexDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId));

            CreateMap<CustomerIndexDto, Customer>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Customer, TopCustomerDto>()
                .ForMember(dest => dest.TotalBalance,
                           opt => opt.MapFrom(src => src.Dispositions.Sum(d => d.Account.Balance)));
        }
    }
}


