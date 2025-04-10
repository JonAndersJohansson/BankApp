using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using System.ComponentModel.DataAnnotations;
using Services.Enums;
using ValidationResult = Services.Enums.ValidationResult;

namespace BankAppProject.Pages.Account
{
    [Authorize(Roles = "Cashier,Admin")]
    public class DepositModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public DepositModel(IAccountService accountService, IMapper mapper)
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
        public DateTime DepositDate { get; set; }

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
            DepositDate = DateTime.Today;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                string operation = "Deposit";
                var status = await _accountService.DepositAsync(AccountId, Amount, Comment, DepositDate, operation);

                if (status == ValidationResult.OK)
                {
                    TempData["DepositMessage"] = $"Deposit successfull";
                    return RedirectToPage("/Account/AccountDetails", new { accountId = AccountId, customerId = CustomerId });
                }

                TempData["ValidationResult"] = $"Input invalid {status}";
                return RedirectToPage("/Account/Deposit", new { accountId = AccountId, customerId = CustomerId });
            }

            TempData["InputInvalid"] = $"Input invalid";
            return RedirectToPage("/Account/Deposit", new { accountId = AccountId, customerId = CustomerId });
        }
    }
}
