using AutoMapper;
using BankAppProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Services.Enums;
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
        public List<SelectListItem>? Frequencies { get; set; }

        public async Task<IActionResult> OnGetAsync(int accountId, int customerId)
        {
            CustomerId = customerId;
            AccountId = accountId;
            Frequencies = _accountService.GetFrequencyList();

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

            Account = _mapper.Map<AccountDetailsViewModel>(accountDto); //här finns frequency
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

        public async Task<IActionResult> OnPostUpdateFrequencyAsync(int accountId, string selectedFrequency)
        {
            if (ModelState.IsValid == false)
            {
                TempData["ErrorUpdatingAccountFrequency"] = $"Account frequency could not be updated. {ModelState.Values.First().Errors.First().ErrorMessage}";
                return RedirectToPage();
            }
            var validation = await _accountService.UpdateFrequencyAsync(accountId, selectedFrequency);
            if (validation != ValidationResult.OK)
            {
                TempData["ErrorUpdatingAccountFrequency"] = $"Account frequency could not be updated. {validation}";
                return RedirectToPage();
            }

            TempData["AccountFrequencyUpdated"] = $"Account frequency updated successfully";
            return RedirectToPage();
        }
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
