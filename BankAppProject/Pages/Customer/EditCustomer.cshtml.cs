using AutoMapper;
using BankAppProject.ViewModels;
using Services.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Services.Enums;

namespace BankAppProject.Pages.Customer
{
    [Authorize(Roles = "Cashier,Admin")]
    public class EditCustomerModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public EditCustomerModel(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [BindProperty]
        public EditCustomerViewModel Customer { get; set; } = new();

        public List<SelectListItem> Genders { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public async Task<IActionResult> OnGetAsync(int customerId)
        {
            Countries = _customerService.GetCountryList();
            Genders = _customerService.GetGenderList();

            var dto = await _customerService.GetCustomerAsync(customerId);
            if (dto == null)
            {
                TempData["NoCustomerFound"] = $"Could not find customer.";
                return RedirectToPage("/Customer/CustomerDetails", new { Customer.CustomerId });
            }

            Customer = _mapper.Map<EditCustomerViewModel>(dto);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<CustomerDetailsDto>(Customer);
                if (dto == null)
                {
                    TempData["NoCustomerFound"] = $"Could not find customer.";
                    return RedirectToPage("/Customer/CustomerDetails", new { customerId = Customer.CustomerId });
                }

                var status = await _customerService.EditCustomerAsync(dto);

                if (status == ValidationResult.OK)
                {
                    TempData["EditCustomerMessage"] = $"Customer updated successfully";
                    return RedirectToPage("/Customer/CustomerDetails", new { customerId = Customer.CustomerId });
                }

                TempData["ValidationErrorMessage"] = $"Validation error. {status}";
                return RedirectToPage("/Customer/EditCustomer", new { customerId = Customer.CustomerId });
            }

            TempData["InvalidInputMessage"] = $"Invalid input.";
            return RedirectToPage("/Customer/EditCustomer", new { customerId = Customer.CustomerId });
        }
        public async Task<IActionResult> OnPostDeleteAsync(int customerId)
        {
            var success = await _customerService.DeleteCustomerAsync(customerId);
            if (!success)
            {
                TempData["ErrorInactivatingCustomer"] = $"Customer could not be inactivated";
                return RedirectToPage("/Customer/CustomerDetails", new { customerId = Customer.CustomerId });
            }

            TempData["InactivatedCustomerMessage"] = $"Customer inactivated successfully";
            return RedirectToPage("/Customer/Index");
        }
    }
}
