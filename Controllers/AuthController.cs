using JobTracker.API.DTOs;
using JobTracker.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthController
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("signup")]
        public async Task<ActionResult<SignupResponsedto>> CreateUserAsync([FromBody] SignupRequestdto request)
        {
            var result = await _authService.CreateUserAsync(request);

            if (result == null)
                return BadRequest("Registration failed");

            return Ok(result);
        }
        [HttpPost("signin")]
        public async Task<ActionResult<SigninResponsedto>> Signin([FromBody] SigninRequestdto request)
        {
            var result = await _authService.SigninAsync(request);

            if (result == null)
                return Unauthorized("Invalid Username or password");

            return Ok(result);
        }
        [HttpPost("forgotpassword")]
        public async Task<ActionResult<CommonResponsedto>> ForgotPassword([FromBody] ForgetPassowrddto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new CommonResponsedto
                {
                    Success = false,
                    Message = "Email is required"
                });

           await _authService.ForgotPasswordAsync(dto.Email);

            return Ok(new CommonResponsedto
            {
                Success = true,
                Message = "If the email exists, a reset link has been sent."
            });
        }

        [HttpPost("resetpassword")]
        public async Task<ActionResult<CommonResponsedto>> ResetPassword([FromBody] ResetPasswordRequestdto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Token) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new CommonResponsedto
                {
                    Success = false,
                    Message = "Token and new password are required"
                });
            }

            var success = await _authService.ResetPasswordAsync(
                dto.Token,
                dto.Password);

            if (!success)
            {
                return BadRequest(new CommonResponsedto
                {
                    Success = false,
                    Message = "Invalid or expired reset token"
                });
            }

            return Ok(new CommonResponsedto
            {
                Success = true,
                Message = "Password reset successfully"
            });
        }
    }

}