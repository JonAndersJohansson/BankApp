using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Account;

namespace BankAppProject.Pages.Account
{
    public class AccountDetailsModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountDetailsModel(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public AccountDetailsViewModel Account { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int accountId)
        {
            var accountDto = await _accountService.GetAccountDetailsAsync(accountId);
            if (accountDto == null)
                return NotFound();

            // Ta endast första 20 transaktioner
            accountDto.Transactions = accountDto.Transactions
                .OrderByDescending(t => t.Date)
                .Take(20)
                .ToList();

            Account = _mapper.Map<AccountDetailsViewModel>(accountDto);
            return Page();
        }
        public async Task<IActionResult> OnGetTransactionsAsync(int accountId, int offset = 0, int pageSize = 20)
        {
            var accountDto = await _accountService.GetAccountDetailsAsync(accountId);
            if (accountDto == null)
                return NotFound();

            var transactions = accountDto.Transactions
                .OrderByDescending(t => t.Date)
                .Skip(offset)
                .Take(pageSize)
                .ToList();

            var viewModels = _mapper.Map<List<TransactionInAccountDetailsViewModel>>(transactions);
            return new JsonResult(viewModels);
        }
        public async Task<IActionResult> OnPostDeleteAsync(int accountId)
        {
            var success = await _accountService.DeleteAccountAsync(accountId);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Account could not be Deleted");
                return Page();
            }

            return RedirectToPage("/Customer/Index");
        }
    }
}
