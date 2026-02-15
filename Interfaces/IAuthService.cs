using JobTracker.API.DTOs;
using JobTracker.API.Models;

namespace JobTracker.API.Interfaces
{
    public interface IAuthService
    {
        Task<SignupResponsedto> CreateUserAsync(SignupRequestdto dto);
        Task<SigninResponsedto?> SigninAsync(SigninRequestdto request);
        Task ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
    }
}
