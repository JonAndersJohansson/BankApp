namespace BankAppProject.ViewModels
{
    public class TopCustomerViewModel
    {
        public int CustomerId { get; set; }
        public string FullName => $"{Givenname} {Surname}";
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
