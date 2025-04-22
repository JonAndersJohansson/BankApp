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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Streetaddress));

            CreateMap<CustomerIndexDto, Customer>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Streetaddress, opt => opt.MapFrom(src => src.Address));

            CreateMap<Customer, TopCustomerDto>()
                .ForMember(dest => dest.TotalBalance,
                           opt => opt.MapFrom(src => src.Dispositions.Sum(d => d.Account.Balance)));


            CreateMap<Customer, CustomerDetailsDto>()
                .ForMember(dest => dest.Accounts,
                    opt => opt.MapFrom(src => src.Dispositions
                        .Where(d => d.Account != null && d.Account.IsActive)
                        .Select(d => d.Account)));

            CreateMap<CustomerDetailsDto, Customer>()
                .ForMember(dest => dest.CountryCode, opt => opt.Ignore())
                .ForMember(dest => dest.Telephonecountrycode, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

        }
    }
}


