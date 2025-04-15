using Microsoft.AspNetCore.Mvc.Rendering;
using Services.DTOs;
using Services.Enums;

namespace Services
{
    public interface IAccountService
    {
        Task<bool> DeleteAccountAsync(int accountId);
        Task<AccountDetailsDto?> GetAccountDetailsAsync(int accountId);
        Task CreateAccountAsync(int customerId);
        List<SelectListItem> GetFrequencyList();
        Task<ValidationResult> UpdateFrequencyAsync(int accountId, string selecteFrequency);
    }
}
