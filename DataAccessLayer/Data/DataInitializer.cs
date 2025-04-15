using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data
{
    public class DataInitializer
    {
        private readonly BankAppDataContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public DataInitializer(BankAppDataContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public void SeedData()
        {
            _dbContext.Database.Migrate();
            SeedRoles();
            SeedUsers();
        }

        // Login
        private void SeedUsers()
        {
            AddUserIfNotExists("richard.chalk@admin.se", "Abc123#", new string[] { "Admin" });
            AddUserIfNotExists("richard.chalk@cashier.se", "Abc123#", new string[] { "Cashier" });
        }

        // Roles
        private void SeedRoles()
        {
            AddRoleIfNotExisting("Admin");
            AddRoleIfNotExisting("Cashier");
        }

        private void AddRoleIfNotExisting(string roleName)
        {
            var role = _dbContext.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                _dbContext.Roles.Add(new IdentityRole { Name = roleName, NormalizedName = roleName });
                _dbContext.SaveChanges();
            }
        }

        private void AddUserIfNotExists(string userName, string password, string[] roles)
        {
            if (_userManager.FindByEmailAsync(userName).Result != null) return;

            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true
            };
            _userManager.CreateAsync(user, password).Wait();
            _userManager.AddToRolesAsync(user, roles).Wait();
        }
    }
}
