using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class AccountDetailsDto
    {
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public DateOnly Created { get; set; }

        public List<TransactionInAccountDetailsDto> Transactions { get; set; } = new List<TransactionInAccountDetailsDto>();
    }

}
