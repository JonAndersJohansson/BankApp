using AutoMapper;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Profiles
{
    public class CustomerServiceProfiles : Profile
    {             
        public CustomerServiceProfiles()
        {
            CreateMap<DataAccessLayer.Models.Customer, CustomerIndexDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId));

            CreateMap<CustomerIndexDto, DataAccessLayer.Models.Customer>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id));
        }
    }
}


