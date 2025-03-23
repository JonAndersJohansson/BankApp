using DataAccessLayer.DTO;

public enum ValidationResult
{
    OK,
    BalanceTooLow,
    IncorrectAmount,
    NoAccountFound,
    DateInPast
}
namespace Services.Account
{
    public interface IAccountService
    {
        Task<bool> DeleteAccountAsync(int accountId);
        Task<AccountDetailsDto?> GetAccountDetailsAsync(int accountId);
        Task<ValidationResult> DepositAsync(int accountId, decimal amount, string comment, DateTime depositDate);
    }
}
