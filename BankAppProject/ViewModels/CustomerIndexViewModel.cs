namespace BankAppProject.ViewModels
{
    public class CustomerIndexViewModel
    {
        public int Id { get; set; }
        public string? NationalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
