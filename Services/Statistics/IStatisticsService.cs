using DataAccessLayer.DTO;

namespace Services.Statistics
{
    public interface IStatisticsService
    {
        Task<List<CountryStatisticsDto>> GetCountryStatisticsAsync();
    }


}
