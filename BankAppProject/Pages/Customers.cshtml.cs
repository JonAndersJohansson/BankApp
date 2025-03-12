using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Customer;

namespace BankAppProject.Pages
{
    public class CustomersModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public CustomersModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public List<CustomersViewModel> Customers { get; set; }
        public int TotalCustomers { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;

        public void OnGet(string sortColumn = "Id", string sortOrder = "asc", int pageNumber = 1)
        {
            PageNumber = pageNumber;
            Customers = _customerService.GetCustomers(sortColumn, sortOrder, PageNumber, PageSize, out int totalCustomers);
            TotalCustomers = totalCustomers;
        }
    }
}
