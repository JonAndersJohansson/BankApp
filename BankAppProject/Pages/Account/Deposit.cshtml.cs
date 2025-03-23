using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Services.Account;
using System.ComponentModel.DataAnnotations;

namespace BankAppProject.Pages.Account
{
    [Authorize(Roles = "Cashier,Admin")]
    [BindProperties]
    public class DepositModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public DepositModel(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        public AccountDetailsViewModel Account { get; set; }

        [Required(ErrorMessage = "Amount required.")]
        [Range(1, 100000)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date required.")]
        public DateTime DepositDate { get; set; }


        [MaxLength(250, ErrorMessage = "Max 50 letters in comment.")]
        public string? Comment { get; set; }

        [BindProperty(SupportsGet = true)]
        public int AccountId { get; set; }

        public async Task OnGetAsync()
        {
            var accountDto = await _accountService.GetAccountDetailsAsync(AccountId);

            if (accountDto == null)
            {
                RedirectToPage("NotFound");
                return;
            }

            Account = _mapper.Map<AccountDetailsViewModel>(accountDto);
            DepositDate = DateTime.Today;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var status = await _accountService.DepositAsync(AccountId, Amount, Comment, DepositDate);

            if (status == ValidationResult.OK)
            {
                return RedirectToPage("AccountDetails", new { accountId = AccountId });
            }

            ModelState.AddModelError(string.Empty, $"Transaction failed: {status}");
            return Page();
        }
    }
}
