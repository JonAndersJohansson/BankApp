using DataAccessLayer.DTO;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetAllCustomers();
        Task<Customer?> GetCustomerByIdAsync(int customerId);
    }

}
