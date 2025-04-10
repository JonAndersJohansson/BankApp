using AutoMapper;
using BankAppProject.ViewModels;
using Services.DTOs;

namespace BankAppProject.Profiles
{
    public class StatisticsProfiles : Profile
    {
        public StatisticsProfiles()
        {
            CreateMap<CountryStatisticsDto, CountryStatisticsViewModel>();
        }
    }
}
