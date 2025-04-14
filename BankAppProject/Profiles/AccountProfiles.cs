using AutoMapper;
using BankAppProject.ViewModels;
using Services.DTOs;
using Services.Enums;

namespace BankAppProject.Profiles
{
    public class AccountProfiles : Profile
    {
        public AccountProfiles()
        {
            //CreateMap<AccountDetailsDto, AccountDetailsViewModel>();

            CreateMap<AccountDetailsDto, AccountDetailsViewModel>()
                .ForMember(dest => dest.Frequency,
                    opt => opt.MapFrom(src => Enum.Parse<Frequence>(src.Frequency, true)))
                    .ReverseMap()
                    .ForMember(dest => dest.Frequency,
                        opt => opt.MapFrom(src => src.Frequency.ToString()));

            CreateMap<TransactionInAccountDetailsDto, TransactionInAccountDetailsViewModel>()
                .ReverseMap();
        }
    }
}
