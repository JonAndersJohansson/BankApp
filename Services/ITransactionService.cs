using DataAccessLayer.Models;
using Services.Enums;

namespace Services
{
    public interface ITransactionService
    {
        Task<ValidationResult> DepositAsync(int accountId, decimal amount, string? comment, DateTime depositDate, string operation);
        Task<ValidationResult> WithdrawAsync(int accountId, decimal amount, string? comment, DateTime withdrawDate, string operation);
        Task<ValidationResult> TransferAsync(int accountId, decimal amount, string? comment, DateTime transferDate, int ReceiverAccountId);
        Task<List<Transaction>> GetRecentTransactionsByCountryAsync(string countryCode, int lastCheckedId);
    }
}
