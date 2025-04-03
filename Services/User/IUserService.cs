using DataAccessLayer.DTO;
using Services.Infrastructure.Paged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.User
{
    public interface IUserService
    {
        //Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(string id);
        Task<PagedResult<UserDto>> GetUsersAsync(string sortColumn, string sortOrder, int pageNumber, int pageSize, string? q);
        Task<bool> DeleteUserAsync(string userId);
    }
}
