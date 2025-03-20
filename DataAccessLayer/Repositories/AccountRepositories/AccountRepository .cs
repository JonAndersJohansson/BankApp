using DataAccessLayer.Data;
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
        private readonly BankAppDataContext _context;

        public AccountRepository(BankAppDataContext context)
        {
            _context = context;
        }

        public async Task<Account> GetByIdAsync(int accountId)
        {
            return await _context.Accounts.FindAsync(accountId);
        }

        public async Task<List<Account>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Accounts
                .Where(a => a.Dispositions.Any(d => d.CustomerId == customerId))
                .ToListAsync();
        }

        public async Task AddAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
        }

        public void Delete(Account account)
        {
            _context.Accounts.Remove(account);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Nya metoder
        public async Task<List<Account>> GetAccountsByCustomerIdAsync(int customerId)
        {
            return await _context.Accounts
                .Where(a => a.Dispositions.Any(d => d.CustomerId == customerId))
                .ToListAsync();
        }

        public void DeleteAccount(Account account)
        {
            _context.Accounts.Remove(account);
        }
    }

}
