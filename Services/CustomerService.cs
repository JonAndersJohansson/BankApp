using AutoMapper;
using Services.DTOs;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.AccountRepositories;
using DataAccessLayer.Repositories.CustomerRepositories;
using DataAccessLayer.Repositories.DispositionRepositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services.Enums;
using Services.Infrastructure.Paged;

namespace Services
{
    public class CustomerService : ICustomerService
    {

        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IDispositionRepository _dispositionRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IAccountRepository accountRepository, IDispositionRepository dispositionRepository, IAccountService accountService, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _dispositionRepository = dispositionRepository;
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task<PagedResult<CustomerIndexDto>> GetCustomersAsync(string sortColumn, string sortOrder, int pageNumber, int pageSize, string? q)
        {
            var query = _customerRepository.GetAllCustomers();

            // Search
            if (!string.IsNullOrWhiteSpace(q))
            {
                string searchQuery = q.Trim().ToLower();

                query = query.Where(c =>
                    c.CustomerId.ToString().Contains(searchQuery) ||
                    c.Givenname.ToLower().Contains(searchQuery) ||
                    c.Surname.ToLower().Contains(searchQuery) ||
                    c.City.ToLower().Contains(searchQuery));
            }


            // Sort
            query = sortColumn switch
            {
                "Id" => sortOrder == "asc" ? query.OrderBy(c => c.CustomerId) : query.OrderByDescending(c => c.CustomerId),
                "National Id" => sortOrder == "asc" ? query.OrderBy(c => c.NationalId) : query.OrderByDescending(c => c.NationalId),
                "Name" => sortOrder == "asc" ? query.OrderBy(c => c.Surname) : query.OrderByDescending(c => c.Surname),
                "Address" => sortOrder == "asc" ? query.OrderBy(c => c.Streetaddress) : query.OrderByDescending(c => c.Streetaddress),
                "City" => sortOrder == "asc" ? query.OrderBy(c => c.City) : query.OrderByDescending(c => c.City),
                _ => query.OrderBy(c => c.CustomerId)  // Standard-sort
            };

            var totalCustomers = await query.CountAsync();

            // Paginering
            var customers = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = _mapper.Map<List<CustomerIndexDto>>(customers);

            return new PagedResult<CustomerIndexDto>
            {
                Results = result,
                RowCount = totalCustomers,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                PageCount = (int)Math.Ceiling((double)totalCustomers / pageSize)
            };
        }
        public async Task<CustomerDetailsDto?> GetCustomerAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null)
                return null;

            return _mapper.Map<CustomerDetailsDto>(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null) return false;

            customer.IsActive = false;

            var dispositions = await _dispositionRepository.GetByCustomerIdAsync(customerId);
            foreach (var disposition in dispositions)
            {
                disposition.IsActive = false;
            }

            var accounts = await _accountRepository.GetAccountsByCustomerIdAsync(customerId);

            foreach (var account in accounts)
            {
                account.IsActive = false;
            }

            await _customerRepository.SaveAsync();
            await _dispositionRepository.SaveAsync();
            await _accountRepository.SaveAsync();

            return true;
        }
        public List<SelectListItem> GetGenderList()
        {
            var Genders = Enum.GetValues<Gender>()
                .Select(g => new SelectListItem
                {
                    Value = ((int)g).ToString(),
                    Text = g.ToString()
                })
                .ToList();
            return Genders;
        }
        public List<SelectListItem> GetCountryList()
        {
            var Countries = Enum.GetValues<Country>()
                .Select(c => new SelectListItem
                {
                    Value = ((int)c).ToString(),
                    Text = c.ToString()
                })
                .ToList();
            return Countries;
        }

        public async Task<(ValidationResult Result, int? CustomerId)> CreateNewCustomerAsync(CustomerDetailsDto newCustomer)
        {
            var validation = ValidateCustomerDto(newCustomer);
            if (validation != ValidationResult.OK)
                return (validation, null);

            var customer = _mapper.Map<Customer>(newCustomer);

            customer.CountryCode = newCustomer.Country switch
            {
                "Sweden" => "SE",
                "Denmark" => "DK",
                "Norway" => "NO",
                "Finland" => "FI",
                _ => ""
            };

            customer.Telephonecountrycode = newCustomer.Country switch
            {
                "Sweden" => "46",
                "Denmark" => "45",
                "Norway" => "47",
                "Finland" => "358",
                _ => ""
            };

            customer.IsActive = true;

            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveAsync();
            await _accountService.CreateAccountAsync(customer.CustomerId);

            return (ValidationResult.OK, customer.CustomerId);
        }

        public async Task<ValidationResult> EditCustomerAsync(CustomerDetailsDto editedCustomer)
        {
            var validation = ValidateCustomerDto(editedCustomer);
            if (validation != ValidationResult.OK)
                return validation;

            var existingCustomer = await _customerRepository.GetCustomerByIdAsync(editedCustomer.CustomerId);
            if (existingCustomer == null)
                return ValidationResult.CustomerNotFound;

            _mapper.Map(editedCustomer, existingCustomer);

            existingCustomer.CountryCode = editedCustomer.Country switch
            {
                "Sweden" => "SE",
                "Denmark" => "DK",
                "Norway" => "NO",
                "Finland" => "FI",
                _ => ""
            };

            existingCustomer.Telephonecountrycode = editedCustomer.Country switch
            {
                "Sweden" => "46",
                "Denmark" => "45",
                "Norway" => "47",
                "Finland" => "358",
                _ => ""
            };

            await _customerRepository.SaveAsync();

            return ValidationResult.OK;
        }

        private ValidationResult ValidateCustomerDto(CustomerDetailsDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Givenname))
                return ValidationResult.MissingGivenName;

            if (string.IsNullOrWhiteSpace(dto.Surname))
                return ValidationResult.MissingSurname;

            if (string.IsNullOrWhiteSpace(dto.Streetaddress))
                return ValidationResult.MissingStreetAddress;

            if (string.IsNullOrWhiteSpace(dto.City))
                return ValidationResult.MissingCity;

            if (string.IsNullOrWhiteSpace(dto.Zipcode))
                return ValidationResult.MissingZipCode;

            if (string.IsNullOrWhiteSpace(dto.Country))
                return ValidationResult.MissingCountry;

            if (string.IsNullOrWhiteSpace(dto.Gender))
                return ValidationResult.MissingGender;

            if (dto.Birthday == null)
                return ValidationResult.MissingBirthday;

            if (dto.Birthday > DateOnly.FromDateTime(DateTime.Today))
                return ValidationResult.InvalidBirthday;

            if (string.IsNullOrWhiteSpace(dto.NationalId))
                return ValidationResult.MissingNationalId;

            if (string.IsNullOrWhiteSpace(dto.Telephonenumber))
                return ValidationResult.MissingPhone;

            if (string.IsNullOrWhiteSpace(dto.Emailaddress))
                return ValidationResult.MissingEmail;

            var countryCode = dto.Country switch
            {
                "Sweden" => "SE",
                "Denmark" => "DK",
                "Norway" => "NO",
                "Finland" => "FI",
                _ => ""
            };

            if (countryCode == "")
                return ValidationResult.InvalidCountry;

            var telephoneCountryCode = dto.Country switch
            {
                "Sweden" => "46",
                "Denmark" => "45",
                "Norway" => "47",
                "Finland" => "358",
                _ => ""
            };

            if (telephoneCountryCode == "")
                return ValidationResult.InvalidTelephoneCountryCode;

            return ValidationResult.OK;
        }
        public async Task<List<TopCustomerDto>> GetTop10RichestCustomersAsync(string countryCode)
        {
            var customers = await _customerRepository.GetTop10RichestCustomersByCountryAsync(countryCode);
            return _mapper.Map<List<TopCustomerDto>>(customers)
                .OrderByDescending(c => c.TotalBalance)
                .Take(10)
                .ToList();
        }

        public async Task<List<string>> GetAllCountryCodesAsync()
        {
            var countryCodes = await _customerRepository.GetAllCountryCodesAsync();
            return countryCodes;
        }
        public async Task<List<Customer>> GetCustomersByCountryAsync(string countryCode)
        {
            return await _customerRepository.GetAllCustomers()
                .Where(c => c.CountryCode == countryCode)
                .ToListAsync();
        }
    }
}
