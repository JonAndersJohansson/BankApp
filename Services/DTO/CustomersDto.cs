using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class CustomersDto
    {
        public int Id { get; set; }
        public string? NationalId { get; set; }
        public string Givenname { get; set; }  // Förnamn
        public string Surname { get; set; }   // Efternamn
        public string Address { get; set; }
        public string City { get; set; }
    }
}
