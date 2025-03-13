namespace BankAppProject.ViewModels
{
    public class CustomersViewModel
    {
        public int Id { get; set; }
        public string? NationalId { get; set; }
        public string? Name { get; set; } //Denna är en sammanslagning av för- och efternamn
        public string Address { get; set; }
        public string City { get; set; }
    }
}
