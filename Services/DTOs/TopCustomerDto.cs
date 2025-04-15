namespace Services.DTOs
{
    public class TopCustomerDto
    {
        public int CustomerId { get; set; }
        public string Givenname { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public decimal TotalBalance { get; set; }
    }
}
