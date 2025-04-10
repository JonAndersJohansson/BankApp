namespace Services.DTOs
{
    public class CustomerIndexDto
    {
        public int Id { get; set; }
        public string? NationalId { get; set; }
        public string Givenname { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
