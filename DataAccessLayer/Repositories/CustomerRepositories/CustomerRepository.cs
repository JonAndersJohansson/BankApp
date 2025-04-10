using DataAccessLayer.Data;
using DataAccessLayer.DTO;
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
        //public async Task<List<TopCustomerDto>> GetTop10RichestCustomersByCountryAsync(string countryCode)
        //{
        //    return await _dbContext.Customers
        //        .Where(c => c.CountryCode == countryCode)
        //        .Include(c => c.Dispositions)
        //        .ThenInclude(d => d.Account)
        //        .Select(c => new TopCustomerDto
        //        {
        //            CustomerId = c.CustomerId,
        //            Givenname = c.Givenname,
        //            Surname = c.Surname,
        //            City = c.City,
        //            Gender = c.Gender,
        //            TotalBalance = c.Dispositions.Sum(d => d.Account.Balance)
        //        })
        //        .OrderByDescending(c => c.TotalBalance)
        //        .Take(10)
        //        .ToListAsync();
        //}
        // DAL: CustomerRepository.cs
        public async Task<List<Customer>> GetTop10RichestCustomersByCountryAsync(string countryCode)
        {
            return await _dbContext.Customers
                .Where(c => c.CountryCode == countryCode)
                .Include(c => c.Dispositions)
                    .ThenInclude(d => d.Account)
                .ToListAsync();
        }

    }
}
