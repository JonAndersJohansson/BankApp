namespace Services.DTOs
{
    public class AccountDetailsDto
    {
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public DateOnly Created { get; set; }
        public string Frequency { get; set; } = null!;
        public List<TransactionInAccountDetailsDto> Transactions { get; set; } = new List<TransactionInAccountDetailsDto>();
    }
}
