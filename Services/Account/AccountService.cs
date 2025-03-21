using DataAccessLayer.DTO;
using DataAccessLayer.Repositories.AccountRepositories;

namespace Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountDetailsDto?> GetAccountDetailsAsync(int accountId)
        {
            var account = await _accountRepository.GetAccountWithTransactionsAsync(accountId);

            if (account == null)
                return null;

            return new AccountDetailsDto
            {
                AccountId = account.AccountId,
                Balance = account.Balance,
                Created = account.Created,
                Transactions = account.Transactions
                    .OrderByDescending(t => t.Date)
                    .Select(t => new TransactionInAccountDetailsDto
                    {
                        TransactionId = t.TransactionId,
                        Date = t.Date,
                        Type = t.Type,
                        Operation = t.Operation,
                        Amount = t.Amount,
                        Balance = t.Balance,
                        Symbol = t.Symbol,
                        Bank = t.Bank
                    }).ToList()
            };
        }
    }
}
