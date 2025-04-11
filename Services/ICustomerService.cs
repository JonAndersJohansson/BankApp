using Services.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Enums;
using Services.Infrastructure.Paged;
using DataAccessLayer.Models;

namespace Services
{
    public interface ICustomerService
    {
        Task<PagedResult<CustomerIndexDto>> GetCustomersAsync(string sortColumn, string sortOrder, int pageNumber, int pageSize, string? q);
        Task<CustomerDetailsDto?> GetCustomerAsync(int customerId);
        Task<bool> DeleteCustomerAsync(int customerId);
        List<SelectListItem> GetGenderList();
        List<SelectListItem> GetCountryList();
        Task<(ValidationResult Result, int? CustomerId)> CreateNewCustomerAsync(CustomerDetailsDto dto);
        Task<ValidationResult> EditCustomerAsync(CustomerDetailsDto dto);
        Task<List<TopCustomerDto>> GetTop10RichestCustomersAsync(string countryCode);
        Task<List<string>> GetAllCountryCodesAsync();
        Task<List<Customer>> GetCustomersByCountryAsync(string countryCode);
    }
}


