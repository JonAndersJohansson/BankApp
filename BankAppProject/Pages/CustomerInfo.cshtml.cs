using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Customer;

namespace BankAppProject.Pages
{
    public class CustomerInfoModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public CustomerInfoModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public CustomersViewModel Customer { get; set; }

        public void OnGet(int id)
        {
            //Customer = _customerService.GetCustomerById(id);
            //if (Customer == null)
            //{
            //    RedirectToPage("/Customers");
            //}
        }
    }
}
