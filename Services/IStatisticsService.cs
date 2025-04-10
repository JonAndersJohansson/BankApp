using DataAccessLayer.DTO;

namespace Services
{
    public interface IStatisticsService
    {
        Task<List<CountryStatisticsDto>> GetCountryStatisticsAsync();
    }


}
