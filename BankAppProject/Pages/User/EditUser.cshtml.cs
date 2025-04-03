using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Customer;
using Services.User;

namespace BankAppProject.Pages.User
{
    public class EditUserModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public EditUserModel(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [BindProperty]
        public UserViewModel User { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            var userDto = await _userService.GetUserByIdAsync(userId);
            if (userDto == null)
            {
                TempData["NoUserFound"] = $"Could not find user.";
                return RedirectToPage("/User/Index");
            }

            User = _mapper.Map<UserViewModel>(userDto);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var userDto = _mapper.Map<UserDto>(User);
                if (userDto == null)
                {
                    TempData["NoUserFound"] = $"Could not find User.";
                    return RedirectToPage("/User/Index");
                }

                var status = await _userService.EditUserAsync(userDto);

                if (status == ValidationResult.OK)
                {
                    TempData["EditUserMessage"] = $"User updated successfully";
                    return RedirectToPage("/User/Index");
                }

                TempData["ValidationErrorMessage"] = $"Validation error. {status}";
                return RedirectToPage("/User/EditUser", new { userId = User.Id });
            }

            TempData["InvalidInputMessage"] = $"Invalid input.";
            return RedirectToPage("/User/EditUser", new { userId = User.Id });
        }
        public async Task<IActionResult> OnPostDeleteAsync(int customerId)
        {
            var success = await _customerService.DeleteCustomerAsync(customerId);
            if (!success)
            {
                TempData["ErrorInactivatingCustomer"] = $"Customer could not be inactivated";
                return RedirectToPage("/Customer/CustomerDetails", new { customerId = Customer.CustomerId });
            }

            TempData["InactivatedCustomerMessage"] = $"Customer inactivated successfully";
            return RedirectToPage("/Customer/Index");
        }
    }
}
