using DataAccessLayer.DTO;
using DataAccessLayer.Repositories.AccountRepositories;
using DataAccessLayer.Repositories.CustomerRepositories;
using DataAccessLayer.Repositories.DispositionRepositories;

namespace Services.Customer
{
    public class CustomerService : ICustomerService
    {

        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IDispositionRepository _dispositionRepository;

        public CustomerService(ICustomerRepository customerRepository, IAccountRepository accountRepository, IDispositionRepository dispositionRepository)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _dispositionRepository = dispositionRepository;
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
                    .Where(d => d.Account != null) // Kontrollera att konto finns
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
    }
}
