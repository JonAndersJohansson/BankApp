using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;

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
