using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using System.ComponentModel.DataAnnotations;
using ValidationResult = Services.Enums.ValidationResult;

namespace BankAppProject.Pages.Account
{
    [Authorize(Roles = "Cashier,Admin")]
    public class WithdrawModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public WithdrawModel(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        [BindProperty(SupportsGet = true)]
        public int AccountId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CustomerId { get; set; }
        public AccountDetailsViewModel Account { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Amount required.")]
        [Range(1, 100000)]
        public decimal Amount { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Date required.")]
        public DateTime WithdrawDate { get; set; }

        [BindProperty]
        [MaxLength(250, ErrorMessage = "Max 50 letters in comment.")]
        public string? Comment { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var accountDto = await _accountService.GetAccountDetailsAsync(AccountId);

            if (accountDto == null)
            {
                TempData["NoAccount"] = $"No account found";
                return RedirectToPage("/Account/AccountDetails", new { accountId = AccountId, customerId = CustomerId });
            }

            Account = _mapper.Map<AccountDetailsViewModel>(accountDto);
            WithdrawDate = DateTime.Today;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var accountDto = await _accountService.GetAccountDetailsAsync(AccountId);

                if (accountDto == null)
                {
                    TempData["NoAccount"] = "No account found";
                    return RedirectToPage("/Account/AccountDetails", new { accountId = AccountId, customerId = CustomerId });
                }

                var account = _mapper.Map<AccountDetailsViewModel>(accountDto);

                if (Amount > account.Balance)
                {
                    TempData["BalanceToLowMessage"] = $"Amount cannot be greater than account balance.";
                    return RedirectToPage("/Account/Withdraw", new { accountId = AccountId, customerId = CustomerId });
                }

                string operation = "Withdraw";
                var status = await _accountService.WithdrawAsync(AccountId, Amount, Comment, WithdrawDate, operation);

                if (status == ValidationResult.OK)
                {
                    TempData["WithdrawMessage"] = $"Withdraw successfull";
                    return RedirectToPage("/Account/AccountDetails", new { accountId = AccountId, customerId = CustomerId });
                }

                TempData["ValidationErrorMessage"] = $"Withdraw error. {status}";
                return RedirectToPage("/Account/Withdraw", new { accountId = AccountId, customerId = CustomerId });
            }

            TempData["ErrorMessage"] = $"Withdraw error. Invalid input.";
            return RedirectToPage("/Account/Withdraw", new { accountId = AccountId, customerId = CustomerId });
        }
    }
}
