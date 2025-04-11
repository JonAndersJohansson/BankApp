using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.CustomerRepositories
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetAllCustomers();
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task SaveAsync();
        Task AddAsync(Customer customer);
        Task<List<Customer>> GetTop10RichestCustomersByCountryAsync(string countryCode);
        Task<List<string>> GetAllCountryCodesAsync();
    }
}
