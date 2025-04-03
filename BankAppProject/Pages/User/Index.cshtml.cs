using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Customer;
using Services.Infrastructure.Paged;
using Services.User;

namespace BankAppProject.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public IndexModel(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public PagedResult<UserViewModel> PagedResult { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string? Q { get; set; }





        public async Task<IActionResult> OnGetAsync(string sortColumn = "Id", string sortOrder = "asc", int pageNumber = 1, string? q = null)
        {
            Q = q;
            PageNumber = pageNumber;

            var pagedResult = await _userService.GetUsersAsync(sortColumn, sortOrder, PageNumber, PageSize, q);

            if (pagedResult == null)
                return RedirectToPage("/Identity/Error");

            var mappedResults = _mapper.Map<List<UserViewModel>>(pagedResult.Results);

            PagedResult = new PagedResult<UserViewModel>
            {
                Results = mappedResults,
                RowCount = pagedResult.RowCount,
                PageSize = pagedResult.PageSize,
                CurrentPage = pagedResult.CurrentPage,
                PageCount = pagedResult.PageCount
            };


            return Page();
        }
    }
}
