using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Customer;

namespace BankAppProject.Pages
{
    public class CustomerInfoModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerInfoViewModel Customer { get; set; } = new();

        public CustomerInfoModel(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var customerDto = await _customerService.GetCustomerAsync(id);
            if (customerDto == null) return NotFound();

            Customer = _mapper.Map<CustomerInfoViewModel>(customerDto);
            return Page();
        }
    }
}
