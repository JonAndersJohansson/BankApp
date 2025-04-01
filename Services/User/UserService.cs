using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.UserRepositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var result = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userRepository.GetRolesAsync(user);
                result.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    Roles = roles.ToList()
                });
            }

            return result;
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            var roles = await _userRepository.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                IsActive = user.IsActive,
                Roles = roles.ToList()
            };
        }
    }
}
