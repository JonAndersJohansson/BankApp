using DataAccessLayer.Models;

namespace Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetRecentTransactionsByCountryAsync(string countryCode, int lastCheckedId);
    }

}
