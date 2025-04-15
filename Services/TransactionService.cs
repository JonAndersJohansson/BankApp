using DataAccessLayer.Models;
using DataAccessLayer.Repositories.AccountRepositories;
using DataAccessLayer.Repositories.TransactionRepositories;
using Microsoft.EntityFrameworkCore;
using Services.Enums;

namespace Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<ValidationResult> DepositAsync(int accountId, decimal amount, string? comment, DateTime depositDate, string operation)
        {
            var account = _accountRepository.GetAccountById(accountId);

            if (account == null || account.IsActive == false)
                return ValidationResult.NoAccountFound;

            if (amount < 1 || amount > 100000)
                return ValidationResult.IncorrectAmount;

            if (depositDate.Date < DateTime.Now.Date)
                return ValidationResult.DateInPast;

            account.Balance += amount;
            await _accountRepository.UpdateAsync(account);

            var transaction = new Transaction
            {
                AccountId = accountId,
                Date = DateOnly.FromDateTime(depositDate),
                Type = "Credit",
                Operation = operation,
                Amount = amount,
                Balance = account.Balance,
                Symbol = comment,
            };
            await _transactionRepository.AddAsync(transaction);

            return ValidationResult.OK;
        }
        public async Task<ValidationResult> WithdrawAsync(int accountId, decimal amount, string? comment, DateTime withdrawDate, string operation)
        {
            var account = _accountRepository.GetAccountById(accountId);

            if (account == null || account.IsActive == false)
                return ValidationResult.NoAccountFound;

            if (amount < 1 || amount > 100000)
                return ValidationResult.IncorrectAmount;

            if (withdrawDate.Date < DateTime.Now.Date)
                return ValidationResult.DateInPast;
            if (account.Balance < amount)
                return ValidationResult.BalanceTooLow;

            account.Balance -= amount;
            await _accountRepository.UpdateAsync(account);

            var transaction = new Transaction
            {
                AccountId = accountId,
                Date = DateOnly.FromDateTime(withdrawDate),
                Type = "Debit",
                Operation = operation,
                Amount = amount,
                Balance = account.Balance,
                Symbol = comment,
            };
            await _transactionRepository.AddAsync(transaction);

            return ValidationResult.OK;
        }
        public async Task<ValidationResult> TransferAsync(int fromAccountId, decimal amount, string? comment, DateTime transferDate, int toAccountId)
        {
            var receivingAccount = _accountRepository.GetAccountById(toAccountId);
            if (receivingAccount == null || !receivingAccount.IsActive)
                return ValidationResult.NoReceivingAccountFound;

            // Withdraw
            var withdrawResult = await WithdrawAsync(fromAccountId, amount, comment, transferDate, $"Transfer to account: {toAccountId}");
            if (withdrawResult != ValidationResult.OK)
                return withdrawResult;

            // Deposit
            var depositResult = await DepositAsync(toAccountId, amount, comment, transferDate, $"Transfer from account: {fromAccountId}");
            if (depositResult != ValidationResult.OK)
                return depositResult;

            return ValidationResult.OK;
        }

        public async Task<List<Transaction>> GetRecentTransactionsByCountryAsync(string countryCode, int lastCheckedId)
        {
            var transactions = await _transactionRepository.GetAllTransactions()
                .Where(t => t.TransactionId > lastCheckedId &&
                            t.AccountNavigation.IsActive &&
                            t.AccountNavigation.Dispositions.Any(d => d.Customer.CountryCode == countryCode))
                .ToListAsync();

            return transactions;
        }


    }

}
