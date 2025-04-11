using DataAccessLayer.Models;
using DataAccessLayer.Repositories.CustomerRepositories;
using DataAccessLayer.Repositories.TransactionRepositories;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<List<Transaction>> GetRecentTransactionsByCountryAsync(string countryCode, int lastCheckedId)
        {
            var transactions = await _transactionRepository.GetAllTransactions()
                .Where(t => t.TransactionId > lastCheckedId &&
                            t.AccountNavigation.IsActive &&
                            t.AccountNavigation.Dispositions.Any(d => d.Customer.CountryCode == countryCode))
                .ToListAsync();

            return transactions;
        }


    }

}
