using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.AccountRepositories
{
    public interface IAccountRepository
    {
        Task<Account> GetByIdAsync(int accountId);
        Task<List<Account>> GetByCustomerIdAsync(int customerId);
        Task AddAsync(Account account);
        void Delete(Account account);
        Task SaveAsync();
        Task<List<Account>> GetAccountsByCustomerIdAsync(int customerId);
        void DeleteAccount(Account account);
    }

}
