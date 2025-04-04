using Services.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankAppProject.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        public Role Role { get; set; }
    }
}
