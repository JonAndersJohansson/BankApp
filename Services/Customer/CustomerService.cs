using DataAccessLayer.DTO;
using DataAccessLayer.Repositories.AccountRepositories;
using DataAccessLayer.Repositories.CustomerRepositories;
using DataAccessLayer.Repositories.DispositionRepositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Account;
using Services.Enums;

namespace Services.Customer
{
    public class CustomerService : ICustomerService
    {

        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IDispositionRepository _dispositionRepository;
        private readonly IAccountService _accountService;

        public CustomerService(ICustomerRepository customerRepository, IAccountRepository accountRepository, IDispositionRepository dispositionRepository, IAccountService accountService)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _dispositionRepository = dispositionRepository;
            _accountService = accountService;
        }

        public List<CustomerIndexDto> GetCustomers(string sortColumn, string sortOrder, int pageNumber, int pageSize, string q, out int totalCustomers)
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


            // Sortering beroende på valda kolumner
            query = sortColumn switch
            {
                "Id" => sortOrder == "asc" ? query.OrderBy(c => c.CustomerId) : query.OrderByDescending(c => c.CustomerId),
                "National Id" => sortOrder == "asc" ? query.OrderBy(c => c.NationalId) : query.OrderByDescending(c => c.NationalId),
                "Name" => sortOrder == "asc" ? query.OrderBy(c => c.Surname) : query.OrderByDescending(c => c.Surname),
                "Address" => sortOrder == "asc" ? query.OrderBy(c => c.Streetaddress) : query.OrderByDescending(c => c.Streetaddress),
                "City" => sortOrder == "asc" ? query.OrderBy(c => c.City) : query.OrderByDescending(c => c.City),
                _ => query.OrderBy(c => c.CustomerId)  // Standard-sortering om inget annat anges
            };

            // Antal kunder totalt för pagination
            totalCustomers = query.Count();

            // Paginering
            var customers = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CustomerIndexDto
                {
                    Id = c.CustomerId,
                    NationalId = c.NationalId,
                    Givenname = c.Givenname,
                    Surname = c.Surname,
                    Address = c.Streetaddress,
                    City = c.City
                }).ToList();

            return customers;
        }
        public async Task<CustomerDetailsDto?> GetCustomerAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null)
                return null;

            Console.WriteLine("Inside GetCustomerAsync in CustomerService");
            

            return new CustomerDetailsDto
            {
                CustomerId = customer.CustomerId,
                Givenname = customer.Givenname,
                Surname = customer.Surname,
                Gender = customer.Gender,
                Streetaddress = customer.Streetaddress,
                City = customer.City,
                Zipcode = customer.Zipcode,
                Country = customer.Country,
                Birthday = customer.Birthday,
                NationalId = customer.NationalId,
                Telephonecountrycode = customer.Telephonecountrycode,
                Telephonenumber = customer.Telephonenumber,
                Emailaddress = customer.Emailaddress,

                // Skapa listan med konton
                Accounts = customer.Dispositions
                    .Where(d => d.Account != null && d.Account.IsActive) // Kontroll IsActive
                    .Select(d => new AccountInCustomerDetailsDto
                    {
                        AccountId = d.Account.AccountId,
                        Balance = d.Account.Balance,
                        Frequency = d.Account.Frequency,
                        Created = d.Account.Created
                    }).ToList()
            };
        }
        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null) return false;

            // Soft delete på kunden
            customer.IsActive = false;

            // Hämta och inaktivera alla dispositioner kopplade till kunden
            var dispositions = await _dispositionRepository.GetByCustomerIdAsync(customerId);
            foreach (var disposition in dispositions)
            {
                disposition.IsActive = false;
            }

            // Hämta och inaktivera alla konton kopplade till kunden
            var accounts = await _accountRepository.GetAccountsByCustomerIdAsync(customerId);
            foreach (var account in accounts)
            {
                account.IsActive = false;
            }

            // Spara ändringar i databasen
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
                    //Value = g.ToString(),
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
                    //Value = c.ToString(),
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

            var countryCode = newCustomer.Country switch
            {
                "Sweden" => "SE",
                "Denmark" => "DK",
                "Norway" => "NO",
                "Finland" => "FI",
                _ => ""
            };

            var telephoneCountryCode = newCustomer.Country switch
            {
                "Sweden" => "46",
                "Denmark" => "45",
                "Norway" => "47",
                "Finland" => "358",
                _ => ""
            };

            var customer = new DataAccessLayer.Models.Customer
            {
                Givenname = newCustomer.Givenname,
                Surname = newCustomer.Surname,
                Gender = newCustomer.Gender,
                Streetaddress = newCustomer.Streetaddress,
                City = newCustomer.City,
                Zipcode = newCustomer.Zipcode,
                Country = newCustomer.Country,
                CountryCode = countryCode,
                Birthday = newCustomer.Birthday,
                NationalId = newCustomer.NationalId,
                Telephonecountrycode = telephoneCountryCode,
                Telephonenumber = newCustomer.Telephonenumber,
                Emailaddress = newCustomer.Emailaddress,
                IsActive = true
            };

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

            existingCustomer.Givenname = editedCustomer.Givenname;
            existingCustomer.Surname = editedCustomer.Surname;
            existingCustomer.Gender = editedCustomer.Gender;
            existingCustomer.Streetaddress = editedCustomer.Streetaddress;
            existingCustomer.City = editedCustomer.City;
            existingCustomer.Zipcode = editedCustomer.Zipcode;
            existingCustomer.Country = editedCustomer.Country;
            existingCustomer.CountryCode = editedCustomer.Country switch
            {
                "Sweden" => "SE",
                "Denmark" => "DK",
                "Norway" => "NO",
                "Finland" => "FI",
                _ => ""
            };
            existingCustomer.Birthday = editedCustomer.Birthday;
            existingCustomer.NationalId = editedCustomer.NationalId;
            existingCustomer.Telephonecountrycode = editedCustomer.Country switch
            {
                "Sweden" => "46",
                "Denmark" => "45",
                "Norway" => "47",
                "Finland" => "358",
                _ => ""
            };
            existingCustomer.Telephonenumber = editedCustomer.Telephonenumber;
            existingCustomer.Emailaddress = editedCustomer.Emailaddress;

            // Spara ändringarna
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
            return await _customerRepository.GetTop10RichestCustomersByCountryAsync(countryCode);
        }


    }
}
