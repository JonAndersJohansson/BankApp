using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.AccountRepositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountWithTransactionsAsync(int accountId);
        Task SaveAsync();
        Task<List<Account>> GetAccountsByCustomerIdAsync(int customerId);
        void DeleteAccount(Account account);
    }

}
