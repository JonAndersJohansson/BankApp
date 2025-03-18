namespace BankAppProject.ViewModels
{
    public class CustomerInfoViewModel
    {
        public int CustomerId { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty; //Baseras på Givenname och Surname
        public string Streetaddress { get; set; } = string.Empty;
        public string ZipcodeCity { get; set; } = string.Empty; //Baseras på Zipcode och City
        public string Country { get; set; } = string.Empty;
        public DateOnly? Birthday { get; set; }
        public int Age => Birthday.HasValue
        ? (DateTime.Today.Year - Birthday.Value.Year -
        (DateTime.Today < Birthday.Value.ToDateTime(TimeOnly.MinValue).AddYears(DateTime.Today.Year - Birthday.Value.Year) ? 1 : 0))
        : 0;    //Baseras på Birthday
        public string? NationalId { get; set; }
        public string? Telephonecountrycode { get; set; }
        public string? Telephonenumber { get; set; }
        public string? Phonenumber => !string.IsNullOrEmpty(Telephonecountrycode) && !string.IsNullOrEmpty(Telephonenumber)
            ? $"+{Telephonecountrycode} {Telephonenumber}"
            : Telephonenumber;      // Sammanslagning av Telephonecountrycode och Telephonenumber
        public string? Emailaddress { get; set; }
        public List<CustomerInfoAccountViewModel> Accounts { get; set; } = new List<CustomerInfoAccountViewModel>();
        public decimal TotalBalance => Accounts.Sum(a => a.Balance);
    }
}
