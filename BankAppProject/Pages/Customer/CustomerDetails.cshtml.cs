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

        public CustomerDetailsModel(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var customerDto = await _customerService.GetCustomerAsync(id);
            if (customerDto == null) return NotFound();

            Customer = _mapper.Map<CustomerDetailsViewModel>(customerDto);
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int customerId)
        {
            var success = await _customerService.DeleteCustomerAsync(customerId);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Customer could not be Deleted");
                return Page();
            }

            return RedirectToPage("/Customer/Index");
        }
    }
}
