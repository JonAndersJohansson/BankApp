using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Customer;
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

        public List<UserViewModel> Users { get; set; }
        public int TotalUsers { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string? Q { get; set; }
        public int FirstRowOnPage { get; set; }
        public int LastRowOnPage { get; set; }
        public int PageCount { get; set; }




        public async Task<IActionResult> OnGetAsync(string sortColumn = "Id", string sortOrder = "asc", int pageNumber = 1, string? q = null)
        {
            Q = q;
            PageNumber = pageNumber;

            var pagedResult = await _userService.GetUsersAsync(sortColumn, sortOrder, PageNumber, PageSize, q);

            if (pagedResult == null)
                return RedirectToPage("/Identity/Error");

            Users = _mapper.Map<List<UserViewModel>>(pagedResult.Results);
            TotalUsers = pagedResult.RowCount;
            PageNumber = pagedResult.CurrentPage;
            PageSize = pagedResult.PageSize;
            FirstRowOnPage = pagedResult.FirstRowOnPage;
            LastRowOnPage = pagedResult.LastRowOnPage;
            PageCount = pagedResult.PageCount;


            return Page();
        }

    }
}
