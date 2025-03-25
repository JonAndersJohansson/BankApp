using DataAccessLayer.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

public enum ValidationResult
{
    OK,
    BalanceTooLow,
    IncorrectAmount,
    NoAccountFound,
    DateInPast,
    NoReceivingAccountFound,
    MissingGivenName,
    MissingSurname,
    MissingStreetAddress,
    MissingCity,
    MissingZipCode,
    MissingCountry,
    MissingGender,
    MissingBirthday,
    InvalidBirthday,
    MissingNationalId,
    MissingPhone,
    MissingEmail,
    InvalidCountry,
    InvalidTelephoneCountryCode,
    CustomerNotFound
}
//public enum Frequency
//{
//    Choose = 0,
//    Daily = 1,
//    Weekly = 2,
//    Monthly = 3,
//    Yearly = 4
//}

namespace Services.Account
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
