using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Customer;

namespace BankAppProject.Pages.Customer
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public IndexModel(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public List<CustomerIndexViewModel> Customers { get; set; }
        public int TotalCustomers { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string? Q { get; set; }

        public IActionResult OnGet(string sortColumn = "Id", string sortOrder = "asc", int pageNumber = 1, string? q = null)
        {
            Q = q;
            PageNumber = pageNumber;
            var customersDto = _customerService.GetCustomers(sortColumn, sortOrder, PageNumber, PageSize, q, out int totalCustomers);
            if(customersDto == null)
                return RedirectToPage("/Identity/Error");

            TotalCustomers = totalCustomers;

            Customers = _mapper.Map<List<CustomerIndexViewModel>>(customersDto);

            return Page();
        }
    }
}
