using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{

    public class CustomerInfoAccountDto
    {
        public int AccountId { get; set; }
        public string Frequency { get; set; } = string.Empty;
        public DateOnly Created { get; set; }
        public decimal Balance { get; set; }
    }
}
