using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.AccountRepositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountWithTransactionsAsync(int accountId);
        Task SaveAsync();
        Task AddAsync(Account account);
        Task<List<Account>> GetAccountsByCustomerIdAsync(int customerId);
        Task<Account?> GetAccountByIdAsync(int accountId);
        Account GetAccountById(int accountId);
        Task UpdateAsync(Account account);
    }
}
