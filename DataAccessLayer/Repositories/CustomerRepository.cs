using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BankAppDataContext _dbContext;

        public CustomerRepository(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Customer> GetAllCustomers()
        {
            return _dbContext.Customers.AsQueryable();
        }
        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            return await _dbContext.Customers
                .Include(c => c.Dispositions)
                .ThenInclude(d => d.Account)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

    }

}
