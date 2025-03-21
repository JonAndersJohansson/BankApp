namespace DataAccessLayer.DTO
{
    public class CustomerDetailsDto
    {
        public int CustomerId { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Givenname { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Streetaddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Zipcode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateOnly? Birthday { get; set; }
        public string? NationalId { get; set; }
        public string? Telephonecountrycode { get; set; }
        public string? Telephonenumber { get; set; }
        public string? Emailaddress { get; set; }
        public List<AccountInCustomerDetailsDto> Accounts { get; set; } = new List<AccountInCustomerDetailsDto>();
        public decimal TotalBalance => Accounts.Sum(a => a.Balance);
    }
}
