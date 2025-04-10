using DataAccessLayer.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Enums;
using Services.Infrastructure.Paged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        //Task<List<UserDto>> GetAllUsersAsync();
        //Task<UserDto> GetUserByIdAsync(string id);
        Task<PagedResult<UserDto>> GetUsersAsync(string sortColumn, string sortOrder, int pageNumber, int pageSize, string? q);
        Task<ValidationResult> DeleteUserAsync(string targetUserId, string currentUserId);
        List<SelectListItem> GetRoleList();
        Task<ValidationResult> UpdateUserRoleAsync(string userId, string selectedRole);
        //Task<ValidationResult> EditUserAsync(UserDto userDto);
    }
}
