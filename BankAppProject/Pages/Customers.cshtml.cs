using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Customer;

namespace BankAppProject.Pages
{
    public class CustomersModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomersModel(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public List<CustomersViewModel> Customers { get; set; }
        public int TotalCustomers { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string Q { get; set; }

        public void OnGet(string sortColumn = "Id", string sortOrder = "asc", int pageNumber = 1, string? q = null)
        {
            Q = q;
            PageNumber = pageNumber;
            var customersDto = _customerService.GetCustomers(sortColumn, sortOrder, PageNumber, PageSize, q, out int totalCustomers);
            TotalCustomers = totalCustomers;

            Customers = _mapper.Map<List<CustomersViewModel>>(customersDto);
        }
    }
}
