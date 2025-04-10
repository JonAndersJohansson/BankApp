using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Account;
using Services.Infrastructure.Paged;

namespace BankAppProject.Pages.Account
{
    [Authorize(Roles = "Cashier,Admin")]
    public class AccountDetailsModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountDetailsModel(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        [BindProperty(SupportsGet = true)]
        public int AccountId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CustomerId { get; set; }

        public AccountDetailsViewModel Account { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int accountId, int customerId)
        {
            CustomerId = customerId;
            AccountId = accountId;

            var accountDto = await _accountService.GetAccountDetailsAsync(accountId);

            if (accountDto == null)
            {
                TempData["NoAccount"] = $"No account found";
                return RedirectToPage("/Customer/CustomerDetails", new { customerId = CustomerId });
            }


            // Ta bara första 20 transaktionerna
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
            {
                TempData["NoAccount"] = $"No account found";
                return RedirectToPage("/Customer/CustomerDetails", new { customerId = CustomerId });
            }

            var page = (offset / pageSize) + 1;

            var transactionsQuery = accountDto.Transactions
                .AsQueryable()
                .OrderByDescending(t => t.Date);

            var paged = transactionsQuery.GetPaged(page, pageSize);

            var viewModels = _mapper.Map<List<TransactionInAccountDetailsViewModel>>(paged.Results);

            var result = new
            {
                results = viewModels,
                currentPage = paged.CurrentPage,
                pageSize = paged.PageSize,
                rowCount = paged.RowCount,
                pageCount = paged.PageCount
            };

            return new JsonResult(result);
        }

        //public async Task<IActionResult> OnGetTransactionsAsync(int accountId, int offset = 0, int pageSize = 20)
        //{
        //    var accountDto = await _accountService.GetAccountDetailsAsync(accountId);
        //    if (accountDto == null)
        //    {
        //        TempData["NoAccount"] = $"No account found";
        //        return RedirectToPage("/Customer/CustomerDetails", new { customerId = CustomerId });
        //    }

        //    var transactions = accountDto.Transactions
        //        .OrderByDescending(t => t.Date)
        //        .Skip(offset)
        //        .Take(pageSize)
        //        .ToList();

        //    var viewModels = _mapper.Map<List<TransactionInAccountDetailsViewModel>>(transactions);
        //    return new JsonResult(viewModels);
        //}
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var success = await _accountService.DeleteAccountAsync(AccountId);

            if (success)
            {
                TempData["InactivatedAccount"] 
                    = $"Account inactivated successfully";
                return RedirectToPage("/Customer/CustomerDetails", 
                    new { customerId = CustomerId });
            }

            TempData["ErrorInactivatingAccount"] = 
                $"Account could not be inactivated";
            return RedirectToPage("/Account/AccountDetails", 
                new { accountId = AccountId, customerId = CustomerId });
        }
    }
}
