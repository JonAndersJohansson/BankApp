using Services.DTOs;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.UserRepositories;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Services.Enums;
using Services.Infrastructure.Paged;
using AutoMapper;

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

        //public async Task<PagedResult<UserDto>> GetUsersAsync(string sortColumn, string sortOrder, int pageNumber, int pageSize, string? q)
        //{
        //    var query = _userRepository.GetAll();

        //    // Sök
        //    if (!string.IsNullOrWhiteSpace(q))
        //    {
        //        var searchQuery = q.Trim().ToLower();
        //        query = query.Where(u =>
        //            u.Email.ToLower().Contains(searchQuery) ||
        //            u.UserName.ToLower().Contains(searchQuery));
        //    }

        //    // Sort
        //    query = sortColumn switch
        //    {
        //        "UserName" => sortOrder == "asc" ? query.OrderBy(u => u.UserName) : query.OrderByDescending(u => u.UserName),
        //        "PhoneNumber" => sortOrder == "asc" ? query.OrderBy(u => u.PhoneNumber) : query.OrderByDescending(u => u.PhoneNumber),
        //        "Email" => sortOrder == "asc" ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email),
        //        "Role" => query.OrderBy(u => u.UserName), // sortera inte
        //        _ => query.OrderBy(u => u.UserName)
        //    };

        //    var totalUsers = await query.CountAsync();

        //    var users = await query
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();

        //    var result = new List<UserDto>();
        //    foreach (var user in users)
        //    {
        //        var role = await _userRepository.GetSingleRoleAsync(user);
        //        result.Add(new UserDto
        //        {
        //            Id = user.Id,
        //            UserName = user.UserName,
        //            PhoneNumber = user.PhoneNumber,
        //            Email = user.Email,
        //            Role = role
        //        });
        //    }

        //    return new PagedResult<UserDto>
        //    {
        //        Results = result,
        //        RowCount = totalUsers,
        //        PageSize = pageSize,
        //        CurrentPage = pageNumber,
        //        PageCount = (int)Math.Ceiling((double)totalUsers / pageSize)
        //    };
        //}
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
                dto.Role = await _userRepository.GetSingleRoleAsync(user); // sätts manuellt
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

        //public async Task<UserDto> GetUserByIdAsync(string id)
        //{
        //    var user = await _userRepository.GetByIdAsync(id);
        //    if (user == null) return null;


        //    return new UserDto
        //    {
        //        Id = user.Id,
        //        UserName = user.UserName,
        //        PhoneNumber = user.PhoneNumber,
        //        Email = user.Email,
        //        IsActive = user.IsActive,
        //        //Role = user.Role,
        //    };
        //}
        public async Task<ValidationResult> DeleteUserAsync(string targetUserId, string currentUserId)
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

        //public async Task<ValidationResult> EditUserAsync(UserDto editedUser)
        //{
        //    return ValidationResult.OK;
        //    var validation = ValidateUserDto(editedUser);
        //    if (validation != ValidationResult.OK)
        //        return validation;

        //    var existingUser = await _userRepository.GetByIdAsync(editedUser.Id);
        //    if (existingUser == null)
        //        return ValidationResult.UserNotFound;

        //    existingUser.UserName = editedUser.UserName;
        //    existingUser.PhoneNumber = editedUser.PhoneNumber;
        //    existingUser.Email = editedUser.Email;

        //    existingUser.Role = editedUser.Role;


        //    // Spara ändringarna
        //    await _userRepository.SaveAsync(existingUser);

        //    return ValidationResult.OK;
        //}

        //private ValidationResult ValidateUserDto(UserDto editedUser)
        //{
        //    if (string.IsNullOrWhiteSpace(editedUser.UserName))
        //        return ValidationResult.MissingGivenName;

        //    if (string.IsNullOrWhiteSpace(editedUser.PhoneNumber))
        //        return ValidationResult.MissingSurname;

        //    if (string.IsNullOrWhiteSpace(dto.Streetaddress))
        //        return ValidationResult.MissingStreetAddress;

        //    if (string.IsNullOrWhiteSpace(dto.City))
        //        return ValidationResult.MissingCity;
        //    return ValidationResult.OK;
        //}
        public List<SelectListItem> GetRoleList()
        {
            return Enum.GetValues<Role>()
                .Select(r => new SelectListItem
                {
                    Value = r.ToString(), // viktig! Inte int
                    Text = r.ToString()
                })
                .ToList();
        }
        //public List<SelectListItem> GetRoleList()
        //{
        //    var Roles = Enum.GetValues<Role>()
        //        .Select(g => new SelectListItem
        //        {
        //            Value = ((int)g).ToString(),
        //            //Value = g.ToString(),
        //            Text = g.ToString()
        //        })
        //        .ToList();
        //    return Roles;
        //}
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
