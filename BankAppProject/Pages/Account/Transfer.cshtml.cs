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


        [Range(1, int.MaxValue, ErrorMessage = "Receiving account must be a positive number.")]
        public int ReceiverAccountId { get; set; }


        [Required(ErrorMessage = "Amount required.")]
        [Range(1, 100000)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date required.")]
        public DateTime TransferDate { get; set; }


        [MaxLength(250, ErrorMessage = "Max 50 letters in comment.")]
        public string? Comment { get; set; }

        public async Task OnGetAsync()
        {
            var accountDto = await _accountService.GetAccountDetailsAsync(AccountId);

            if (accountDto == null)
            {
                RedirectToPage("NotFound");
                return;
            }

            Account = _mapper.Map<AccountDetailsViewModel>(accountDto);
            TransferDate = DateTime.Today;
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

            var status = await _accountService.TransferAsync(AccountId, Amount, Comment, TransferDate, ReceiverAccountId);

            if (status == ValidationResult.OK)
            {
                TempData["TransferMessage"] = $"Transfer successfull";
                return RedirectToPage("/Account/AccountDetails", new { accountId = AccountId, customerId = CustomerId });
            }

            ModelState.AddModelError(string.Empty, $"Transaction failed: {status}");
            return Page();
        }

    }
}
