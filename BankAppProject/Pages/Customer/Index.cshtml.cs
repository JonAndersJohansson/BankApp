using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Infrastructure.Paged;

namespace BankAppProject.Pages.Customer
{
    [Authorize(Roles = "Cashier,Admin")]
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public IndexModel(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public PagedResult<CustomerIndexViewModel> PagedResult { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string? Q { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortColumn = "Id", string sortOrder = "asc", int pageNumber = 1, string? q = null)
        {
            Q = q;
            PageNumber = pageNumber;

            var pagedResult = await _customerService.GetCustomersAsync(sortColumn, sortOrder, PageNumber, PageSize, q);

            if (pagedResult == null)
                return RedirectToPage("/Identity/Error");

            var mappedResults = _mapper.Map<List<CustomerIndexViewModel>>(pagedResult.Results);

            PagedResult = new PagedResult<CustomerIndexViewModel> 
            {
                Results = mappedResults,
                RowCount = pagedResult.RowCount,
                PageSize = pagedResult.PageSize,
                CurrentPage = pagedResult.CurrentPage,
                PageCount = pagedResult.PageCount
            };

            return Page(); //PagedResuld har Address här
        }
    }
}
