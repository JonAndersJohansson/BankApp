using Microsoft.AspNetCore.Mvc.Rendering;
using Services.DTOs;
using Services.Enums;
using Services.Infrastructure.Paged;

namespace Services
{
    public interface IUserService
    {
        Task<PagedResult<UserDto>> GetUsersAsync(string sortColumn, string sortOrder, int pageNumber, int pageSize, string? q);
        Task<ValidationResult> DeleteUserAsync(string targetUserId, string? currentUserId);
        List<SelectListItem> GetRoleList();
        Task<ValidationResult> UpdateUserRoleAsync(string userId, string selectedRole);
    }
}
