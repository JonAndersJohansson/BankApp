using DataAccessLayer.Repositories.StatisticsRepositories;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;

namespace Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public StatisticsService(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }
        public async Task<List<CountryStatisticsDto>> GetCountryStatisticsAsync()
        {
            try
            {
                var query = from c in _statisticsRepository.GetCustomers()
                            join d in _statisticsRepository.GetDispositions() on c.CustomerId equals d.CustomerId
                            join a in _statisticsRepository.GetAccounts() on d.AccountId equals a.AccountId
                            group new { c, a } by c.CountryCode into countryGroup
                            select new CountryStatisticsDto
                            {
                                CountryCode = countryGroup.Key,
                                TotalClients = countryGroup.Select(g => g.c.CustomerId).Distinct().Count(),
                                TotalAccounts = countryGroup.Select(g => g.a.AccountId).Distinct().Count(),
                                TotalCapital = countryGroup.Sum(g => g.a.Balance)
                            };

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel i GetCountryStatisticsAsync: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return new List<CountryStatisticsDto>();
            }
        }
    }
}
