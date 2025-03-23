using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Account;
using System.ComponentModel.DataAnnotations;

namespace BankAppProject.Pages.Account
{
    [Authorize(Roles = "Cashier,Admin")]
    [BindProperties]
    public class DepositModel : PageModel
    {
        private readonly IAccountService _accountService;

        public DepositModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [Required(ErrorMessage = "Amount required.")]
        [Range(1, 100000)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date required.")]
        public DateTime DepositDate { get; set; }


        [MaxLength(250, ErrorMessage = "Max 50 letters in comment.")]
        public string Comment { get; set; }

        public void OnGet(int accountId)
        {
            DepositDate = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync(int accountId)
        {
            if (!ModelState.IsValid)
                return Page();

            var status = await _accountService.DepositAsync(accountId, Amount, Comment, DepositDate);

            if (status == ValidationResult.OK)
            {
                return RedirectToPage("AccountDetails", new { id = accountId });
            }

            // Lägg till ett felmeddelande om något affärsregel-fel uppstod
            ModelState.AddModelError(string.Empty, $"Transaction failed: {status}");
            return Page();
        }

    }
}
