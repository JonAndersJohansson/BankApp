using DataAccessLayer.Repositories;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public StatisticsService(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        public List<CountryStatisticsDto> GetCountryStatistics()
        {
            var customers = _statisticsRepository.GetCustomers().ToList();
            var dispositions = _statisticsRepository.GetDispositions().ToList();
            var accounts = _statisticsRepository.GetAccounts().ToList();

            return customers
                .GroupBy(c => c.CountryCode)
                .Select(g => new CountryStatisticsDto
                {
                    CountryCode = g.Key,
                    TotalClients = g.Count(),
                    TotalAccounts = dispositions.Count(d => g.Select(c => c.CustomerId).Contains(d.CustomerId)),
                    TotalCapital = accounts.Where(a => dispositions
                                        .Where(d => g.Select(c => c.CustomerId).Contains(d.CustomerId))
                                        .Select(d => d.AccountId)
                                        .Contains(a.AccountId))
                                        .Sum(a => a.Balance)
                })
                .ToList();
        }

    }


}
