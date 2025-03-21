namespace BankAppProject.ViewModels
{
    public class CustomerIndexViewModel
    {
        public int Id { get; set; }
        public string? NationalId { get; set; }
        public string? Name { get; set; } //sammanslagning
        public string Address { get; set; }
        public string City { get; set; }
    }
}
