using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Customer;
using System.ComponentModel.DataAnnotations;

namespace BankAppProject.Pages.Customer
{
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

        [BindProperty]
        public int CustomerId { get; set; }



        //[Range(1, 99, ErrorMessage = "Invalid")]
        //public Gender CustomerGender { get; set; }
        public List<SelectListItem> Genders { get; set; }

        //[Range(1, 10, ErrorMessage = "Invalid")]
        //public Country CustomerCountry { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public async Task<IActionResult> OnGetAsync(int customerId)
        {
            CustomerId = customerId;
            Countries = _customerService.GetCountryList();
            Genders = _customerService.GetGenderList();

            var dto = await _customerService.GetCustomerAsync(customerId);
            if (dto == null) return NotFound();

            Customer = _mapper.Map<EditCustomerViewModel>(dto);

            //if (!Enum.TryParse(Customer.CustomerGender, out Gender gender))
            //    gender = Gender.Choose; // fallback default

            //CustomerGender = gender;

            //if (!Enum.TryParse(Customer.Country, out Country country))
            //    country = Country.Choose;

            //CustomerCountry = country;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<CustomerDetailsDto>(Customer);
                //dto.Telephonecountrycode = CustomerCountry switch
                //{
                //    Country.Sweden => "+46",
                //    Country.Norway => "+47",
                //    Country.Denmark => "+45",
                //    Country.Finland => "+358",
                //    _ => ""
                //};

                var (status, customerId) = await _customerService.EditCustomerAsync(dto);

                if (status == ValidationResult.OK && customerId.HasValue)
                {
                    TempData["EditCustomerMessage"] = $"Customer updated successfully";
                    return RedirectToPage("/Customer/CustomerDetails", new { customerId = CustomerId });
                }

                ModelState.AddModelError(string.Empty, $"Update customer failed: {status}");

                //CustomerGender = Gender.Choose;
                //CustomerCountry = Country.Choose;
                Countries = _customerService.GetCountryList();
                Genders = _customerService.GetGenderList();
                return Page();
            }

            //CustomerGender = Gender.Choose;
            //CustomerCountry = Country.Choose;
            Countries = _customerService.GetCountryList();
            Genders = _customerService.GetGenderList();
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
            TempData["InactivatedCustomerMessage"] = $"Customer inactivated successfully";
            return RedirectToPage("/Customer/Index");
        }
    }
}
