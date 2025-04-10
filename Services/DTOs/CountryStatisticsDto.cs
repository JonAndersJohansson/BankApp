namespace Services.DTOs
{
    public class CountryStatisticsDto
    {
        public string CountryCode { get; set; }
        public int TotalClients { get; set; }
        public int TotalAccounts { get; set; }
        public decimal TotalCapital { get; set; }
    }

}
