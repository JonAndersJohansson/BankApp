namespace Services.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
