using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;

namespace BankAppProject.Profiles
{
    public class AccountProfiles : Profile
    {
        public AccountProfiles()
        {
            CreateMap<AccountDetailsDto, AccountDetailsViewModel>();

            CreateMap<TransactionInAccountDetailsDto, TransactionInAccountDetailsViewModel>();
        }
    }
}
