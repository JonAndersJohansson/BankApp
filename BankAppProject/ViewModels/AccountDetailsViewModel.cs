using Services.Enums;

namespace BankAppProject.ViewModels
{
    public class AccountDetailsViewModel
    {
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public DateOnly Created { get; set; }

        public Frequence Frequency { get; set; }
        public List<TransactionInAccountDetailsViewModel> Transactions { get; set; } = new List<TransactionInAccountDetailsViewModel>();
    }

}
