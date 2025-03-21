using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;

namespace BankAppProject.Profiles
{
    public class AccountDetailsProfile : Profile
    {
        public AccountDetailsProfile()
        {
            CreateMap<AccountDetailsDto, AccountDetailsViewModel>();

            CreateMap<TransactionInAccountDetailsDto, TransactionInAccountDetailsViewModel>();
        }
    }
}
