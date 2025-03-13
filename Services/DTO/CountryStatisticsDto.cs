using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class CountryStatisticsDto
    {
        public string CountryCode { get; set; }
        public int TotalClients { get; set; }
        public int TotalAccounts { get; set; }
        public decimal TotalCapital { get; set; }
    }

}
