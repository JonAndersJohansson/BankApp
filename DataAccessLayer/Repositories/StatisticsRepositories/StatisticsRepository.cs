using DataAccessLayer.Data;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.StatisticsRepositories
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
