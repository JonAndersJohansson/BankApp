using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IQueryable<ApplicationUser> GetAll()
        {
            return _userManager.Users.Where(u => u.IsActive);
        }
        public async Task<string?> GetSingleRoleAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
                return "Admin";

            return roles.FirstOrDefault();

        }


        //public Task <List<ApplicationUser>> GetAllAsync()
        //{
        //    return _userManager.Users
        //        .Where(u => u.IsActive)
        //        .ToListAsync();
        //}

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _userManager
                .FindByIdAsync(id);
        }
        public async Task SaveAsync(ApplicationUser user)
        {
            await _userManager.UpdateAsync(user);
        }

    }
}
