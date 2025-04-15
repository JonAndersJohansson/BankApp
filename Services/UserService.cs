using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.UserRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Enums;
using Services.Infrastructure.Paged;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserDto>> GetUsersAsync(string sortColumn, string sortOrder, int pageNumber, int pageSize, string? q)
        {
            var query = _userRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var searchQuery = q.Trim().ToLower();
                query = query.Where(u =>
                    u.Email.ToLower().Contains(searchQuery) ||
                    u.UserName.ToLower().Contains(searchQuery));
            }

            query = sortColumn switch
            {
                "UserName" => sortOrder == "asc" ? query.OrderBy(u => u.UserName) : query.OrderByDescending(u => u.UserName),
                "PhoneNumber" => sortOrder == "asc" ? query.OrderBy(u => u.PhoneNumber) : query.OrderByDescending(u => u.PhoneNumber),
                "Email" => sortOrder == "asc" ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email),
                "Role" => query.OrderBy(u => u.UserName), // sortera inte på roll
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
                var dto = _mapper.Map<UserDto>(user);
                dto.Role = await _userRepository.GetSingleRoleAsync(user);
                result.Add(dto);
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

        public async Task<ValidationResult> DeleteUserAsync(string targetUserId, string? currentUserId)
        {
            if (targetUserId == currentUserId)
                return ValidationResult.CannotDeleteSelf;

            var user = await _userRepository.GetByIdAsync(targetUserId);
            if (user == null)
                return ValidationResult.UserNotFound;

            user.IsActive = false;
            await _userRepository.SaveAsync(user);

            return ValidationResult.OK;
        }

        public List<SelectListItem> GetRoleList()
        {
            return Enum.GetValues<Role>()
                .Select(r => new SelectListItem
                {
                    Value = r.ToString(), // inte int
                    Text = r.ToString()
                })
                .ToList();
        }
 
        public async Task<ValidationResult> UpdateUserRoleAsync(string userId, string selectedRole)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return ValidationResult.UserNotFound;

            if (string.IsNullOrWhiteSpace(selectedRole))
                return ValidationResult.NoSelectedRole;

            if (!await _roleManager.RoleExistsAsync(selectedRole))
                return ValidationResult.InvalidRole;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return ValidationResult.UserNotFound;

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (!currentRoles.Any())
                return ValidationResult.NoRoleFound;

            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, selectedRole);

            return ValidationResult.OK;
        }
    }
}
