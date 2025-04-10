using AutoMapper;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


