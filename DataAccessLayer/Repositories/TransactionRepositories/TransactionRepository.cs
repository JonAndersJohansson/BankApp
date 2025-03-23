using DataAccessLayer.Data;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.TransactionRepositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankAppDataContext _bankAppDataContext;

        public TransactionRepository(BankAppDataContext bankAppDataContext)
        {
            _bankAppDataContext = bankAppDataContext;
        }
        public async Task AddAsync(Transaction transaction)
        {
            _bankAppDataContext.Transactions.Add(transaction);
            await _bankAppDataContext.SaveChangesAsync();
        }
    }
}
