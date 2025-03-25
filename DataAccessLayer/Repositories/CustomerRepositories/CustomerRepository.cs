using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.CustomerRepositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.CustomerrRepositories
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
            return _dbContext.Customers
                .Where(c => c.IsActive)
                .AsQueryable();

        }
        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            return await _dbContext.Customers
                .Include(c => c.Dispositions)
                .ThenInclude(d => d.Account)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }
        //public void Delete(Customer customer)
        //{
        //    _dbContext.Customers.Remove(customer);
        //}
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
        }
    }
}
