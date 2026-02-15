using JobTracker.API.DTOs;
using JobTracker.API.Models;
using Microsoft.AspNetCore.Identity;

namespace JobTracker.API.Interfaces
{
    public interface IAuthRepository
    {
        Task<UsersLoginRecord> SigninAsync(string username);
        Task<int> CreateUserAsync(UsersLoginRecord user);
        Task<UsersLoginRecord?> GetUserByEmailAsync(string email);
        Task<UsersLoginRecord?> GetUserByResetTokenAsync(string token);

        Task SavePasswordResetTokenAsync(int userId, string token, DateTime expiry);
        Task ClearPasswordResetTokenAsync(int userId);
        Task UpdatePasswordAsync(int userId, string passwordHash);
    }
}
