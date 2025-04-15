using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.TransactionRepositories
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
        IQueryable<Transaction> GetAllTransactions();
    }
}
