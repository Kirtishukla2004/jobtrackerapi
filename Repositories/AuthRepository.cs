using System.Data;
using System.Threading.Tasks;
using JobTracker.API.Data;
using JobTracker.API.DTOs;
using JobTracker.API.Interfaces;
using JobTracker.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

public class AuthRepository : IAuthRepository
{
    private readonly DBHelper _dbHelper;
   
    public AuthRepository(DBHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }
    public async Task<int> CreateUserAsync(UsersLoginRecord user)
    {
        var hasher = new PasswordHasher<object>();
        var parameters = new[]
        {
            new SqlParameter("@Name", user.Name),
            new SqlParameter("@Username", user.Username),
            new SqlParameter("@passwordHash", user.PasswordHash)
        };
        List<int> result;
        try
        {
            result = await _dbHelper.ExecuteStoredProcedureAsync<int>("sp_CreateUser", parameters);
        }
        catch (Exception ex)
        {
            throw;
        }
        return result.FirstOrDefault();
    }
    public async Task<UsersLoginRecord> SigninAsync(string username)
    {
        var users = await _dbHelper.ExecuteStoredProcedureAsync<UsersLoginRecord>("sp_GetUserByEmail", new[] { new SqlParameter("@Email", username) });

        return users.FirstOrDefault();
    }

    public async Task<UsersLoginRecord?> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;
        var users = await _dbHelper.ExecuteStoredProcedureAsync<UsersLoginRecord>("sp_GetUserByEmail",new[] { new SqlParameter("@Email", email) });
        return users.FirstOrDefault();
    }

    public async Task<UsersLoginRecord?> GetUserByResetTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }
        var users = await _dbHelper.ExecuteStoredProcedureAsync<UsersLoginRecord>("sp_GetUserByResetToken", new[] { new SqlParameter("@Token", token) });
        return users.FirstOrDefault();
    }

    public async Task SavePasswordResetTokenAsync(int userId,string token,DateTime expiry)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid userId", nameof(userId));

        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentNullException(nameof(token));

        if (expiry <= DateTime.UtcNow)
            throw new ArgumentException("Expiry must be in the future", nameof(expiry));
        var parameters = new[]
            {
            new SqlParameter("@UserId", userId),
            new SqlParameter("@Token", token),
            new SqlParameter("@Expiry", expiry)
            };
        await _dbHelper.ExecuteStoredProcedureAsync<int>("sp_SaveResetPasswordToken", parameters);
    }
    public async Task UpdatePasswordAsync(int userId, string passwordHash)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid userId", nameof(userId));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentNullException(nameof(passwordHash));
        var parameters = new[]
            {
            new SqlParameter("@UserId", userId),
            new SqlParameter("@PasswordHash", passwordHash)
            };
        await _dbHelper.ExecuteStoredProcedureAsync<int>("sp_UpdateUserPassword", parameters);
    }
    public async Task ClearPasswordResetTokenAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid userId", nameof(userId));
        var parameter = new[]
            {
            new SqlParameter("@UserId", userId)
            };
        await _dbHelper.ExecuteStoredProcedureAsync<int>("sp_ClearPasswordResetToken",parameter);
    }

}
