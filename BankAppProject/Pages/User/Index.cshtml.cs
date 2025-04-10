using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Services;
using Services.Enums;
using Services.Infrastructure.Paged;

namespace BankAppProject.Pages.User
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(IUserService userService, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public PagedResult<UserViewModel> PagedResult { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string? Q { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortColumn = "Id", string sortOrder = "asc", int pageNumber = 1, string? q = null)
        {
            Roles = _userService.GetRoleList();
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
        public async Task<IActionResult> OnPostUpdateRoleAsync(string userId, string selectedRole)
        {
            if (ModelState.IsValid == false)
            {
                TempData["ErrorUpdatingUserRole"] = $"User role could not be updated. {ModelState.Values.First().Errors.First().ErrorMessage}";
                return RedirectToPage();
            }
            var validation = await _userService.UpdateUserRoleAsync(userId, selectedRole);
            if (validation != ValidationResult.OK)
            {
                TempData["ErrorUpdatingUserRole"] = $"User role could not be updated. {validation}";
                return RedirectToPage();
            }

            TempData["UserRoleUpdated"] = $"User role updated successfully";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorInactivatingUser"] = $"User could not be inactivated";
                return RedirectToPage();
            }

            var currentUserId = _userManager.GetUserId(User); //ClaimsPrincipal!

            var validation = await _userService.DeleteUserAsync(userId, currentUserId);

            if (validation != ValidationResult.OK)
            {
                TempData["ErrorInactivatingUser"] = $"User could not be inactivated. {validation}";
                return RedirectToPage();
            }

            TempData["InactivatedUser"] = $"User inactivated successfully";
            return RedirectToPage();
        }

    }
}
