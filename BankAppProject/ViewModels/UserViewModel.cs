using Services.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankAppProject.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public Role Role { get; set; }
    }
}
