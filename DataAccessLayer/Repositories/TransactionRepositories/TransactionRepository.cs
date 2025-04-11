using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

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
        public IQueryable<Transaction> GetAllTransactions()
        {
            return _bankAppDataContext.Transactions
                .Include(t => t.AccountNavigation)
                    .ThenInclude(a => a.Dispositions)
                        .ThenInclude(d => d.Customer);
        }

    }
}
