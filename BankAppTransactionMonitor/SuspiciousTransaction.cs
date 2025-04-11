namespace BankAppTransactionMonitor
{
    public class SuspiciousTransaction
    {
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public string Rule { get; set; } = string.Empty;

    }
}
