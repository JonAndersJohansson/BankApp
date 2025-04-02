using DataAccessLayer.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Enums;

namespace Services.Customer
{
    public interface ICustomerService
    {
        List<CustomerIndexDto> GetCustomers(string sortColumn, string sortOrder, int pageNumber, int pageSize, string q, out int totalCustomers);

        Task<CustomerDetailsDto?> GetCustomerAsync(int customerId);
        Task<bool> DeleteCustomerAsync(int customerId);
        List<SelectListItem> GetGenderList();
        List<SelectListItem> GetCountryList();
        Task<(ValidationResult Result, int? CustomerId)> CreateNewCustomerAsync(CustomerDetailsDto dto);
        Task<ValidationResult> EditCustomerAsync(CustomerDetailsDto dto);
        Task<List<TopCustomerDto>> GetTop10RichestCustomersAsync(string countryCode);
    }
}


