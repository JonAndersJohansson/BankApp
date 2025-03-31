using BankAppProject.ViewModels;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Customer;
using Services.Enums;

namespace BankAppProject.Pages.Customer
{
    public class NewCustomerModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public NewCustomerModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [BindProperty]
        public NewCustomerViewModel Customer { get; set; } = new();


        public List<SelectListItem> Genders { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public void OnGet()
        {
            Countries = _customerService.GetCountryList();
            Genders = _customerService.GetGenderList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var newCustomer = new CustomerDetailsDto
                {
                    Givenname = Customer.Givenname,
                    Surname = Customer.Surname,
                    Gender = Customer.CustomerGender.ToString(),
                    Streetaddress = Customer.StreetAddress,
                    Zipcode = Customer.ZipCode,
                    City = Customer.City,
                    Country = Customer.CustomerCountry.ToString(),
                    Birthday = DateOnly.FromDateTime(Customer.Birthday!.Value),
                    NationalId = Customer.NationalId,
                    Telephonenumber = Customer.Telephonenumber,
                    Emailaddress = Customer.Emailaddress
                };
                var (status, customer) = await _customerService.CreateNewCustomerAsync(newCustomer);

                if (status == ValidationResult.OK && customer.HasValue)
                {
                    TempData["NewCustomerMessage"] = $"Customer created successfully";
                    return RedirectToPage("CustomerDetails", new { customerId = customer });
                }

                TempData["ValidationFailedMessage"] = $"New customer failed: {status}";
                return RedirectToPage("/Customer/NewCustomer");
            }

            TempData["InvalidInputMessage"] = $"New customer failed. Invalig input";
            return RedirectToPage("/Customer/NewCustomer");
        }
    }
}
