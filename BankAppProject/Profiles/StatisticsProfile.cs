using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;

namespace BankAppProject.Profiles
{
    public class StatisticsProfile : Profile
    {
        public StatisticsProfile()
        {
            CreateMap<CountryStatisticsDto, CountryStatisticsViewModel>();
        }
    }

}
