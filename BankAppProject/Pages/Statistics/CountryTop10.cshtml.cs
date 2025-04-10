using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace BankAppProject.Pages.Statistics
{
    [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "countryCode" })]
    public class CountryTop10Model : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CountryTop10Model(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public List<TopCustomerViewModel>? Top10Customers { get; set; }
        public string? CountryCode { get; set; }

        public async Task OnGetAsync(string countryCode)
        {
            CountryCode = countryCode;
            var dtos = await _customerService.GetTop10RichestCustomersAsync(countryCode);
            Top10Customers = _mapper.Map<List<TopCustomerViewModel>>(dtos);
        }
    }
}
