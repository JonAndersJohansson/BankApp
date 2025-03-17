using DataAccessLayer.DTO;

namespace Services.Customer
{
    public interface ICustomerService
    {
        List<CustomersDto> GetCustomers(string sortColumn, string sortOrder, int pageNumber, int pageSize, out int totalCustomers);
    }
}


