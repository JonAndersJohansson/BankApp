using DataAccessLayer.DTO;

namespace Services.Account
{
    public interface IAccountService
    {
        Task<AccountDetailsDto?> GetAccountDetailsAsync(int accountId);
    }
}
