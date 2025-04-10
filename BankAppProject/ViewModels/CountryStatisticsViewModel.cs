namespace BankAppProject.ViewModels
{
    public class CountryStatisticsViewModel
    {
        public string CountryCode { get; set; } = string.Empty;
        public int TotalClients { get; set; }
        public int TotalAccounts { get; set; }
        public decimal TotalCapital { get; set; }
    }

}
