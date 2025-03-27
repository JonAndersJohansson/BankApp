using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Customer;

namespace BankAppProject.Pages.Customer
{
    public class CustomerDetailsModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerDetailsViewModel Customer { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int CustomerId { get; set; }

        public CustomerDetailsModel(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }


        public async Task<IActionResult> OnGetAsync(int customerId)
        {
            CustomerId = customerId;

            var customerDto = await _customerService.GetCustomerAsync(customerId);
            if (customerDto == null) return NotFound();

            Customer = _mapper.Map<CustomerDetailsViewModel>(customerDto);
            return Page();
        }
    }
}
