using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.AccountRepositories;
using DataAccessLayer.Repositories.TransactionRepositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.DTOs;
using Services.Enums;

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

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null) return false;

            account.IsActive = false;

            await _accountRepository.SaveAsync();

            return true;
        }

        public async Task CreateAccountAsync(int customerId)
        {
            var account = new Account
            {
                Created = DateOnly.FromDateTime(DateTime.Today),
                Frequency = "Monthly",
                Balance = 0,
                IsActive = true,
                Dispositions = new List<Disposition>()
            };

            account.Dispositions.Add(new Disposition
            {
                CustomerId = customerId,
                Type = "OWNER"
            });

            await _accountRepository.AddAsync(account);
            await _accountRepository.SaveAsync();
        }

        public List<SelectListItem> GetFrequencyList()
        {
            var Frequencies = Enum.GetValues<Frequence>()
                .Select(f => new SelectListItem
                {
                    Value = ((int)f).ToString(),
                    Text = f.ToString()
                })
                .ToList();
            return Frequencies;
        }

        public async Task<ValidationResult> UpdateFrequencyAsync(int accountId, string selectedFrequency)
        {
            if (accountId <= 0)
                return ValidationResult.NoAccountFound;

            if (!Enum.TryParse<Frequence>(selectedFrequency, out var frequencyEnum) || !Enum.IsDefined(typeof(Frequence), frequencyEnum))
            {
                return ValidationResult.NoSelectedFrequency;
            }

            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account == null)
                return ValidationResult.NoAccountFound;

            account.Frequency = frequencyEnum.ToString();

            await _accountRepository.UpdateAsync(account);

            return ValidationResult.OK;
        }
    }
}
