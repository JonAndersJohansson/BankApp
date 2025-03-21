using DataAccessLayer.Data;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Nya metoder
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
        //public IQueryable<Account> GetAllAccounts()
        //{
        //    return _dbContext.Accounts
        //        .Where(c => c.IsActive)
        //        .AsQueryable();

        //}

    }

}
