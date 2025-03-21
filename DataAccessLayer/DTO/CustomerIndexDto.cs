namespace DataAccessLayer.DTO
{
    public class CustomerIndexDto
    {
        public int Id { get; set; }
        public string? NationalId { get; set; }
        public string Givenname { get; set; }  // Förnamn
        public string Surname { get; set; }   // Efternamn
        public string Address { get; set; }
        public string City { get; set; }
    }
}
