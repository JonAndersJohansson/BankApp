using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly BankAppDataContext _dbContext;

        public CustomerService(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CustomersViewModel> GetCustomers(string sortColumn, string sortOrder, int pageNumber, int pageSize, out int totalCustomers)
        {
            var query = _dbContext.Customers.AsQueryable();

            if (sortColumn == "Id")
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.CustomerId);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.CustomerId);

            if (sortColumn == "NationalId")
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.NationalId);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.NationalId);

            if (sortColumn == "Name")
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.Surname);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Surname);

            if (sortColumn == "Address")
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.Streetaddress);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Streetaddress);

            if (sortColumn == "City")
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.City);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.City);

            // Antal kunder totalt för pagination
            totalCustomers = query.Count();

            var customers = query
                .Skip((pageNumber - 1) * pageSize)  // Hoppa över tidigare sidor
                .Take(pageSize)  // Ta endast de kunder vi behöver
                .Select(c => new CustomersViewModel
                {
                    Id = c.CustomerId,
                    NationalId = c.NationalId,
                    Name = c.Givenname + " " + c.Surname,
                    Address = c.Streetaddress,
                    City = c.City
                }).ToList();

            return customers;
        }
    }
}
