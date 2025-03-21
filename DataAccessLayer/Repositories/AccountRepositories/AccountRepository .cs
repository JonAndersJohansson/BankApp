using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.AccountRepositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankAppDataContext _dbContext;

        public AccountRepository(BankAppDataContext context)
        {
            _dbContext = context;
        }

        public async Task<Account> GetByIdAsync(int accountId)
        {
            return await _dbContext.Accounts.FindAsync(accountId);
        }

        public async Task<List<Account>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbContext.Accounts
                .Where(a => a.Dispositions.Any(d => d.CustomerId == customerId))
                .ToListAsync();
        }

        public async Task AddAsync(Account account)
        {
            await _dbContext.Accounts.AddAsync(account);
        }


        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Account>> GetAccountsByCustomerIdAsync(int customerId)
        {
            return await _dbContext.Accounts
                .Where(a => a.Dispositions.Any(d => d.CustomerId == customerId))
                .ToListAsync();
        }

        public void DeleteAccount(Account account)
        {
            _dbContext.Accounts.Remove(account);
        }
        public async Task<Account?> GetAccountWithTransactionsAsync(int accountId)
        {
            return await _dbContext.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }
    }
}
