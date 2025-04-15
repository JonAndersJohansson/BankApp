using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;
    }
}
