using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.CustomerRepositories
{
    public interface ICustomerRepository
    {
        //void Delete(Customer customer);
        IQueryable<Customer> GetAllCustomers();
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task SaveAsync();
        Task AddAsync(Customer customer);

    }

}
