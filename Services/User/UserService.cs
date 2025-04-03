using DataAccessLayer.DTO;
using DataAccessLayer.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Services.Infrastructure.Paged;

namespace Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PagedResult<UserDto>> GetUsersAsync(string sortColumn, string sortOrder, int pageNumber, int pageSize, string? q)
        {
            var query = _userRepository.GetAll();

            // Sök
            if (!string.IsNullOrWhiteSpace(q))
            {
                var searchQuery = q.Trim().ToLower();
                query = query.Where(u =>
                    u.Email.ToLower().Contains(searchQuery) ||
                    u.UserName.ToLower().Contains(searchQuery));
            }

            // Sort
            query = sortColumn switch
            {
                "UserName" => sortOrder == "asc" ? query.OrderBy(u => u.UserName) : query.OrderByDescending(u => u.UserName),
                "PhoneNumber" => sortOrder == "asc" ? query.OrderBy(u => u.PhoneNumber) : query.OrderByDescending(u => u.PhoneNumber),
                "Email" => sortOrder == "asc" ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email),
                "Role" => query.OrderBy(u => u.UserName), // sortera ej
                _ => query.OrderBy(u => u.UserName)
            };

            var totalUsers = await query.CountAsync();

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new List<UserDto>();
            foreach (var user in users)
            {
                var role = await _userRepository.GetSingleRoleAsync(user);
                result.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Role = role
                });
            }

            return new PagedResult<UserDto>
            {
                Results = result,
                RowCount = totalUsers,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                PageCount = (int)Math.Ceiling((double)totalUsers / pageSize)
            };

        }




        //public async Task<List<UserDto>> GetUsersAsync(string sortColumn, string sortOrder, int pageNumber, int pageSize, string? q, out int totalUsers)
        //{
        //    var query = await _userRepository.GetAll();

        //    // Search
        //    if (!string.IsNullOrWhiteSpace(q))
        //    {
        //        string searchQuery = q.Trim().ToLower();

        //        query = query.Where(u =>
        //            u.Email.ToString().Contains(searchQuery) ||
        //            u.UserName.ToLower().Contains(searchQuery));
        //    }


        //    // Sortering beroende på valda kolumner
        //    query = sortColumn switch
        //    {
        //        "UserName" => sortOrder == "asc" ? query.OrderBy(u => u.UserName) : query.OrderByDescending(u => u.UserName),
        //        "PhoneNumber" => sortOrder == "asc" ? query.OrderBy(u => u.PhoneNumber) : query.OrderByDescending(u => u.PhoneNumber),
        //        "Email" => sortOrder == "asc" ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email),
        //        "Role" => sortOrder == "asc" ? query.OrderBy(u => u.Role) : query.OrderByDescending(u => u.Role),
        //        _ => query.OrderBy(u => u.UserName)  // Standard-sortering om inget annat anges
        //    };

        //    // Antal users totalt för pagination
        //    totalUsers = query.Count();

        //    // Paginering
        //    var users = query
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .Select(u => new UserDto
        //        {
        //            UserName = u.UserName,
        //            PhoneNumber = u.PhoneNumber,
        //            Email = u.Email,
        //            Role = u.Role,
        //        }).ToList();

        //    return users;
        //}


        //public async Task<List<UserDto>> GetAllUsersAsync()
        //{
        //    var users = await _userRepository.GetAllAsync();
        //    var result = new List<UserDto>();

        //    foreach (var user in users)
        //    {
        //        var roles = await _userRepository.GetRolesAsync(user);
        //        result.Add(new UserDto
        //        {
        //            Id = user.Id,
        //            UserName = user.UserName,
        //            PhoneNumber = user.PhoneNumber,
        //            Email = user.Email,
        //            IsActive = user.IsActive,
        //            Roles = roles.ToList()
        //        });
        //    }

        //    return result;
        //}

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;


            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                IsActive = user.IsActive,
                //Role = user.Role,
            };
        }
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = false;

            await _userRepository.SaveAsync(user);

            return true;
        }
    }
}
