using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Services.DTOs;
using Services.Enums;

namespace BankAppProject.Pages.Customer
{
    [Authorize(Roles = "Cashier,Admin")]
    public class NewCustomerModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public NewCustomerModel(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [BindProperty]
        public NewCustomerViewModel Customer { get; set; } = new();


        public List<SelectListItem>? Genders { get; set; }
        public List<SelectListItem>? Countries { get; set; }

        public void OnGet()
        {
            Countries = _customerService.GetCountryList();
            Genders = _customerService.GetGenderList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var newCustomer = _mapper.Map<CustomerDetailsDto>(Customer);

                //var newCustomer = new CustomerDetailsDto
                //{
                //    Givenname = Customer.Givenname,
                //    Surname = Customer.Surname,
                //    Gender = Customer.CustomerGender.ToString(),
                //    Streetaddress = Customer.StreetAddress,
                //    Zipcode = Customer.ZipCode,
                //    City = Customer.City,
                //    Country = Customer.CustomerCountry.ToString(),
                //    Birthday = DateOnly.FromDateTime(Customer.Birthday!.Value),
                //    NationalId = Customer.NationalId,
                //    Telephonenumber = Customer.Telephonenumber,
                //    Emailaddress = Customer.Emailaddress
                //};
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
