using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class TopCustomerDto
    {
        public int CustomerId { get; set; }
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
