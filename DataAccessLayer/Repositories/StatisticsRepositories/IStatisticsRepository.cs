using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.StatisticsRepositories
{
    public interface IStatisticsRepository
    {
        IQueryable<Customer> GetCustomers();
        IQueryable<Account> GetAccounts();
        IQueryable<Disposition> GetDispositions();
    }


}
