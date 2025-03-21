using DataAccessLayer.DTO;

namespace Services.Customer
{
    public interface ICustomerService
    {
        List<CustomerIndexDto> GetCustomers(string sortColumn, string sortOrder, int pageNumber, int pageSize, string q, out int totalCustomers);

        Task<CustomerDetailsDto?> GetCustomerAsync(int customerId);
        Task<bool> DeleteCustomerAsync(int customerId);
    }
}


