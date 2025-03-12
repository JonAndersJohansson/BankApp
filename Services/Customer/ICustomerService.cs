using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Customer
{
    public interface ICustomerService
    {
        List<CustomersViewModel> GetCustomers(string sortColumn, string sortOrder, int pageNumber, int pageSize, out int totalCustomers);
    }
}


