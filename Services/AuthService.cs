using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobTracker.API.DTOs;
using JobTracker.API.Interfaces;
using JobTracker.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using static System.Net.WebRequestMethods;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _userRepo;
    private readonly PasswordHasher<object> _hasher = new();
    private readonly IEmailServices _emailServices;

    public AuthService(
        IAuthRepository userRepo,
        IEmailServices emailServices)
    {
        _userRepo = userRepo;
        _emailServices = emailServices;
    }

    private Tokendto GenerateToken(UsersLoginRecord user)
    {
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEYJOBTRACKER")
            ?? throw new InvalidOperationException("JWT_KEYJOBTRACKER missing");

        var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer")
            ?? throw new InvalidOperationException("Jwt__Issuer missing");

        var jwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience")
            ?? throw new InvalidOperationException("Jwt__Audience missing");

        var expiresInMinutesStr =
            Environment.GetEnvironmentVariable("ExpiresInMinutes") ?? "60";

        if (!int.TryParse(expiresInMinutesStr, out var expiresInMinutes))
            expiresInMinutes = 60;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.PreferredUsername, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new Tokendto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expires
        };
    }

    public async Task<SignupResponsedto> CreateUserAsync(SignupRequestdto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return new SignupResponsedto { Message = "Name is required" };

        if (string.IsNullOrWhiteSpace(dto.Username))
            return new SignupResponsedto { Message = "Email is required" };

        if (string.IsNullOrWhiteSpace(dto.Password))
            return new SignupResponsedto { Message = "Password is required" };

        var user = new UsersLoginRecord
        {
            Name = dto.Name,
            Username = dto.Username,
            PasswordHash = _hasher.HashPassword(null, dto.Password),
            IsActive = true
        };

        try
        {
            await _userRepo.CreateUserAsync(user);
            return new SignupResponsedto { Message = "User created successfully" };
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("UNIQUE KEY constraint"))
            {
                return new SignupResponsedto
                {
                    Message = "Email already exists"
                };
            }
            throw;
        }
    }

    public async Task<SigninResponsedto?> SigninAsync(SigninRequestdto request)
    {
        var user = await _userRepo.SigninAsync(request.Username);

        if (user == null || !user.IsActive)
            return null;

        var result = _hasher.VerifyHashedPassword(
            null,
            user.PasswordHash,
            request.Password
        );

        if (result != PasswordVerificationResult.Success)
            return null;

        var tokenDto = GenerateToken(user);

        return new SigninResponsedto
        {
            Token = tokenDto.Token,
            ExpiresAt = tokenDto.ExpiresAt,
            Name = user.Name,
            Username = user.Username
        };
    }

    public async Task ForgotPasswordAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return;

        var user = await _userRepo.GetUserByEmailAsync(email);
        if (user == null)
            return;

        var token = Guid.NewGuid().ToString("N");
        var expiry = DateTime.UtcNow.AddMinutes(15);

        await _userRepo.SavePasswordResetTokenAsync(user.Id, token, expiry);

        var frontendUrl =
            Environment.GetEnvironmentVariable("FRONTEND_URL")
            ?? "https://jobtracker-indol.vercel.app";

        var resetLink = $"{https://jobtracker-indol.vercel.app}/resetpassword?token={token}";

        await _emailServices.SendAsync(
            user.Username,
            "Reset your password",
            $@"
            <p>Click the link below to reset your password:</p>
            <p><a href='{resetLink}'>{resetLink}</a></p>
            <p>This link expires in 15 minutes.</p>"
        );
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(token) ||
            string.IsNullOrWhiteSpace(newPassword))
            return false;

        var user = await _userRepo.GetUserByResetTokenAsync(token);
        if (user == null)
            return false;

        if (user.ResetPasswordTokenExpiry == null ||
            user.ResetPasswordTokenExpiry < DateTime.UtcNow)
            return false;

        var hashedPassword = _hasher.HashPassword(null, newPassword);

        await _userRepo.UpdatePasswordAsync(user.Id, hashedPassword);
        await _userRepo.ClearPasswordResetTokenAsync(user.Id);

        return true;
    }
}
