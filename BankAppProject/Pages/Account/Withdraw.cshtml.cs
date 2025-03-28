using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Account;
using System.ComponentModel.DataAnnotations;
using ValidationResult = Services.Enums.ValidationResult;

namespace BankAppProject.Pages.Account
{
    [Authorize(Roles = "Cashier,Admin")]
    [BindProperties]
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

        [Required(ErrorMessage = "Amount required.")]
        [Range(1, 100000)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date required.")]
        public DateTime WithdrawDate { get; set; }


        [MaxLength(250, ErrorMessage = "Max 50 letters in comment.")]
        public string? Comment { get; set; }

        //[BindProperty(SupportsGet = true)]
        //public int AccountId { get; set; }

        public async Task OnGetAsync()
        {
            var accountDto = await _accountService.GetAccountDetailsAsync(AccountId);

            if (accountDto == null)
            {
                RedirectToPage("NotFound");
                return;
            }

            Account = _mapper.Map<AccountDetailsViewModel>(accountDto);
            WithdrawDate = DateTime.Today;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var accountDto = await _accountService.GetAccountDetailsAsync(AccountId);
            if (accountDto == null)
            {
                return RedirectToPage("NotFound");
            }

            Account = _mapper.Map<AccountDetailsViewModel>(accountDto);

            if (!ModelState.IsValid)
                return Page();

            if (Amount > Account.Balance)
            {
                ModelState.AddModelError(nameof(Amount), "Amount cannot be greater than account balance.");
                return Page();
            }
            string operation = "Withdraw";
            var status = await _accountService.WithdrawAsync(AccountId, Amount, Comment, WithdrawDate, operation);

            if (status == ValidationResult.OK)
            {
                TempData["WithdrawMessage"] = $"Withdraw successfull";
                return RedirectToPage("/Account/AccountDetails", new { accountId = AccountId, customerId = CustomerId });
            }

            ModelState.AddModelError(string.Empty, $"Transaction failed: {status}");
            return Page();
        }

    }
}
