using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public interface IStatisticsRepository
    {
        IQueryable<Customer> GetCustomers();
        IQueryable<Account> GetAccounts();
        IQueryable<Disposition> GetDispositions();
    }


}
