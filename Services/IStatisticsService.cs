using Services.DTOs;

namespace Services
{
    public interface IStatisticsService
    {
        Task<List<CountryStatisticsDto>> GetCountryStatisticsAsync();
    }


}
