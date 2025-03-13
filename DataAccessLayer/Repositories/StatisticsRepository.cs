using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly BankAppDataContext _dbContext;

        public StatisticsRepository(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Customer> GetCustomers()
        {
            return _dbContext.Customers;
        }

        public IQueryable<Account> GetAccounts()
        {
            return _dbContext.Accounts;
        }

        public IQueryable<Disposition> GetDispositions()
        {
            return _dbContext.Dispositions;
        }
    }


}
