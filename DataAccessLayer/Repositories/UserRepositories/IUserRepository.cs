using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.UserRepositories
{
    public interface IUserRepository
    {
        IQueryable<ApplicationUser> GetAll();
        Task<string?> GetSingleRoleAsync(ApplicationUser user);
        Task<ApplicationUser?> GetByIdAsync(string id);
    }
}
