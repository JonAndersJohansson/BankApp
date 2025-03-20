using DataAccessLayer.DTO;
using DataAccessLayer.Repositories;

namespace Services.Customer
{
    public class CustomerService : ICustomerService
    {

        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<CustomersDto> GetCustomers(string sortColumn, string sortOrder, int pageNumber, int pageSize, string q, out int totalCustomers)
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
                "NationalId" => sortOrder == "asc" ? query.OrderBy(c => c.NationalId) : query.OrderByDescending(c => c.NationalId),
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
                .Select(c => new CustomersDto
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
        public async Task<CustomerInfoDto?> GetCustomerAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null)
                return null;

            Console.WriteLine($"Antal Dispositions: {customer.Dispositions.Count}");
            Console.WriteLine($"Antal konton: {customer.Dispositions.Count(d => d.Account != null)}");

            return new CustomerInfoDto
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
                    .Select(d => new CustomerInfoAccountDto
                    {
                        AccountId = d.Account.AccountId,
                        Balance = d.Account.Balance,
                        Frequency = d.Account.Frequency,
                        Created = d.Account.Created
                    }).ToList()
            };
        }

    }
}
