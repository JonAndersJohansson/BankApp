using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.TransactionRepositories
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
        //Task<List<Transaction>> GetTransactionsByCountrySinceAsync(string countryCode, int lastCheckedId);
        IQueryable<Transaction> GetAllTransactions();
    }
}
