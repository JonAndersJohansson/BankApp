using Services.DTOs;
using Services.Enums;

namespace Services
{
    public interface IAccountService
    {
        Task<bool> DeleteAccountAsync(int accountId);
        Task<AccountDetailsDto?> GetAccountDetailsAsync(int accountId);
        Task<ValidationResult> DepositAsync(int accountId, decimal amount, string comment, DateTime depositDate, string operation);
        Task<ValidationResult> WithdrawAsync(int accountId, decimal amount, string comment, DateTime withdrawDate, string operation);
        Task<ValidationResult> TransferAsync(int accountId, decimal amount, string? comment, DateTime transferDate, int ReceiverAccountId);
        Task CreateAccountAsync(int customerId);
    }
}
