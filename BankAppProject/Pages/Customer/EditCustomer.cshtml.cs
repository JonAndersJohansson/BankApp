using AutoMapper;
using BankAppProject.ViewModels;
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

        [Range(1, 99, ErrorMessage = "Invalid")]
        public Gender CustomerGender { get; set; }
        public List<SelectListItem> Genders { get; set; }

        [Range(1, 10, ErrorMessage = "Invalid")]
        public Country CustomerCountry { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Countries = _customerService.GetCountryList();
            Genders = _customerService.GetGenderList();

            var dto = await _customerService.GetCustomerAsync(id);
            if (dto == null) return NotFound();

            Customer = _mapper.Map<EditCustomerViewModel>(dto);

            Countries = _customerService.GetCountryList();
            Genders = _customerService.GetGenderList();

            return Page();

            //      Givenname
            //      Surname
            //      CustomerGender
            //       StreetAddress
            //        ZipCode
            //        City
            //        CustomerCountry
            //       Birthday
            //       NationalId
            //       Telephonenumber
            //       Emailaddress
        }
    }
}
