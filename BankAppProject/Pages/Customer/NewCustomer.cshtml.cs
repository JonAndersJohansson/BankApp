using DataAccessLayer.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Customer;
using System.ComponentModel.DataAnnotations;

namespace BankAppProject.Pages.Customer
{
    [BindProperties]
    public class NewCustomerModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public NewCustomerModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [MaxLength(30, ErrorMessage = "First name not valid, to long")]
        [Required(ErrorMessage = "First name required.")]
        public string Givenname { get; set; }

        [MaxLength(30, ErrorMessage = "Last name not valid, to long")]
        [Required(ErrorMessage = "Last name required.")]
        public string Surname { get; set; }

        [Range(1, 99, ErrorMessage = "Invalid")]
        public Gender CustomerGender { get; set; }
        public List<SelectListItem> Genders { get; set; }

        [MaxLength(50, ErrorMessage = "Street address not valid, to long.")]
        [Required(ErrorMessage = "Street address required.")]
        public string StreetAddress { get; set; }

        [MaxLength(8, ErrorMessage = "Zipcode not valid.")]
        [Required(ErrorMessage = "Zipcode required.")]
        public string ZipCode { get; set; }

        [MaxLength(20, ErrorMessage = "City name is to long.")]
        [Required(ErrorMessage = "City required.")]
        public string City { get; set; }

        [Range(1, 10, ErrorMessage = "Invalid")]
        public Country CustomerCountry { get; set; }
        public List<SelectListItem> Countries { get; set; }

        [Required(ErrorMessage = "Birthday required.")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }


        [MaxLength(12, ErrorMessage = "Social security number not valid.")]
        [Required(ErrorMessage = "Social security number required.")]
        public string NationalId { get; set; }

        [MaxLength(20, ErrorMessage = "Phone number not valid, to long.")]
        [Required(ErrorMessage = "Phone number required.")]
        public string Telephonenumber { get; set; }

        [MaxLength(50, ErrorMessage = "Email address not valid, to long.")]
        [Required(ErrorMessage = "Email address required.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]

        public string Emailaddress { get; set; }

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
                    Givenname = Givenname,
                    Surname = Surname,
                    Gender = CustomerGender.ToString(),
                    Streetaddress = StreetAddress,
                    Zipcode = ZipCode,
                    City = City,
                    Country = CustomerCountry.ToString(),
                    Birthday = DateOnly.FromDateTime(Birthday!.Value),
                    NationalId = NationalId,
                    Telephonenumber = Telephonenumber,
                    Emailaddress = Emailaddress
                };
                var status = await _customerService.CreateNewCustomer(newCustomer);

                if (status == ValidationResult.OK)
                {
                    return RedirectToPage("Index");
                }

                ModelState.AddModelError(string.Empty, $"Transaction failed: {status}");

                Birthday = new DateTime(1990, 1, 1);
                Countries = _customerService.GetCountryList();
                Genders = _customerService.GetGenderList();
                return Page();
            }
            Birthday = new DateTime(1990, 1, 1);
            Countries = _customerService.GetCountryList();
            Genders = _customerService.GetGenderList();
            return Page();
        }
    }
}
