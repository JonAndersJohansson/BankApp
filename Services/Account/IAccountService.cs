using DataAccessLayer.DTO;

namespace Services.Account
{
    public interface IAccountService
    {
        Task<bool> DeleteAccountAsync(int accountId);
        Task<AccountDetailsDto?> GetAccountDetailsAsync(int accountId);
    }
}
