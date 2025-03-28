using DataAccessLayer.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.Customer
{
    public enum Gender
    {
        Choose = 0,
        Male = 1,
        Female = 2,
        Other = 3
    }
    public enum Country
    {
        Choose = 0,
        Finland = 1,
        Denmark = 2,
        Norway = 3,
        Sweden = 4
    }
    public interface ICustomerService
    {
        List<CustomerIndexDto> GetCustomers(string sortColumn, string sortOrder, int pageNumber, int pageSize, string q, out int totalCustomers);

        Task<CustomerDetailsDto?> GetCustomerAsync(int customerId);
        Task<bool> DeleteCustomerAsync(int customerId);
        List<SelectListItem> GetGenderList();
        List<SelectListItem> GetCountryList();
        Task<(ValidationResult Result, int? CustomerId)> CreateNewCustomerAsync(CustomerDetailsDto dto);
        Task<ValidationResult> EditCustomerAsync(CustomerDetailsDto dto);
    }
}


