namespace BankAppProject.ViewModels
{
    public class CustomerInfoAccountViewModel
    {
        public int AccountId { get; set; }
        public string Frequency { get; set; } = string.Empty;
        public DateOnly Created { get; set; }
        public decimal Balance { get; set; }
    }
}
