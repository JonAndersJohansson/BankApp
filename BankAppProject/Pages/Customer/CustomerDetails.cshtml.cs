using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Account;
using Services.Customer;

namespace BankAppProject.Pages.Customer
{
    [Authorize(Roles = "Cashier,Admin")]
    public class CustomerDetailsModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public CustomerDetailsModel(ICustomerService customerService, IAccountService accountService, IMapper mapper)
        {
            _customerService = customerService;
            _accountService = accountService;
            _mapper = mapper;
        }
        public CustomerDetailsViewModel Customer { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int CustomerId { get; set; }

        public async Task<IActionResult> OnGetAsync(int customerId)
        {
            CustomerId = customerId;

            var customerDto = await _customerService.GetCustomerAsync(customerId);
            if (customerDto == null) return NotFound();

            Customer = _mapper.Map<CustomerDetailsViewModel>(customerDto);
            return Page();
        }
        public async Task<IActionResult> OnPostAddAccountAsync(int customerId)
        {
            await _accountService.CreateAccountAsync(customerId);

            TempData["CreatedAccountMessage"] = $"Account created successfully";
            return RedirectToPage("/Customer/CustomerDetails", new { customerId = customerId });
        }
    }
}
