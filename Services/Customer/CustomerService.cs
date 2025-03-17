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

    }
}
