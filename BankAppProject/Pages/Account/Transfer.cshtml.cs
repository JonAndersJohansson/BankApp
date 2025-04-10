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
    public class TransferModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public TransferModel(IAccountService accountService, IMapper mapper)
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
        [Range(1, int.MaxValue, ErrorMessage = "Receiving account must be a positive number.")]
        public int ReceiverAccountId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Amount required.")]
        [Range(1, 100000)]
        public decimal Amount { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Date required.")]
        public DateTime TransferDate { get; set; }

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
            TransferDate = DateTime.Today;
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
                    TempData["InvalidBalanceMessage"] = $"Could not complete transaction. Balance to low.";
                    return RedirectToPage("/Account/Transfer", new { accountId = AccountId, customerId = CustomerId });
                }

                var status = await _accountService.TransferAsync(AccountId, Amount, Comment, TransferDate, ReceiverAccountId);

                if (status == ValidationResult.OK)
                {
                    TempData["TransferMessage"] = $"Transfer successfull";
                    return RedirectToPage("/Account/AccountDetails", new { accountId = AccountId, customerId = CustomerId });
                }

                TempData["ValidationErrorMessage"] = $"Transfer error. {status}";
                return RedirectToPage("/Account/Transfer", new { accountId = AccountId, customerId = CustomerId });
            }

            TempData["ErrorMessage"] = $"Transfer error. Invalid input.";
            return RedirectToPage("/Account/Transfer", new { accountId = AccountId, customerId = CustomerId });
        }
    }
}
