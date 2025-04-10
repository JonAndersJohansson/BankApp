using Services.DTOs;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.AccountRepositories;
using DataAccessLayer.Repositories.TransactionRepositories;
using Services.Enums;
using AutoMapper;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<AccountDetailsDto?> GetAccountDetailsAsync(int accountId)
        {
            var account = await _accountRepository.GetAccountWithTransactionsAsync(accountId);

            if (account == null)
                return null;

            var dto = _mapper.Map<AccountDetailsDto>(account);
            dto.Transactions = dto.Transactions.OrderByDescending(t => t.Date).ToList();

            return dto;
        }


        //public async Task<AccountDetailsDto?> GetAccountDetailsAsync(int accountId)
        //{
        //    var account = await _accountRepository.GetAccountWithTransactionsAsync(accountId);

        //    if (account == null)
        //        return null;

        //    return new AccountDetailsDto
        //    {
        //        AccountId = account.AccountId,
        //        Balance = account.Balance,
        //        Created = account.Created,
        //        Transactions = account.Transactions
        //            .OrderByDescending(t => t.Date)
        //            .Select(t => new TransactionInAccountDetailsDto
        //            {
        //                TransactionId = t.TransactionId,
        //                Date = t.Date,
        //                Type = t.Type,
        //                Operation = t.Operation,
        //                Amount = t.Amount,
        //                Balance = t.Balance,
        //                Symbol = t.Symbol,
        //                Bank = t.Bank
        //            }).ToList()
        //    };
        //}
        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null) return false;

            account.IsActive = false;

            await _accountRepository.SaveAsync();

            return true;
        }
        public async Task<ValidationResult> DepositAsync(int accountId, decimal amount, string comment, DateTime depositDate, string operation)
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
        public async Task<ValidationResult> WithdrawAsync(int accountId, decimal amount, string comment, DateTime withdrawDate, string operation)
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
        public async Task<ValidationResult> TransferAsync(int fromAccountId, decimal amount, string comment, DateTime transferDate, int toAccountId)
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
        public async Task CreateAccountAsync(int customerId)
        {
            var account = new Account
            {
                Created = DateOnly.FromDateTime(DateTime.Today),
                Frequency = "Monthly",
                Balance = 0,
                IsActive = true
            };

            account.Dispositions.Add(new Disposition
            {
                CustomerId = customerId,
                Type = "OWNER"
            });

            await _accountRepository.AddAsync(account);
            await _accountRepository.SaveAsync();
        }



    }
}
