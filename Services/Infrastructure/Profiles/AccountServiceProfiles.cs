using AutoMapper;
using DataAccessLayer.Models;
using Services.DTOs;

namespace Services.Infrastructure.Profiles
{
    public class AccountServiceProfiles : Profile
    {
        public AccountServiceProfiles()
        {
            CreateMap<Account, AccountInCustomerDetailsDto>();

            CreateMap<Account, AccountDetailsDto>();

        }
    }
}
