using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public interface IStatisticsRepository
    {
        IQueryable<Customer> GetCustomers();
        IQueryable<Account> GetAccounts();
        IQueryable<Disposition> GetDispositions();
    }


}
