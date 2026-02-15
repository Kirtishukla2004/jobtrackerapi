using System.Security.Claims;
using JobTracker.API.DTOs;
using JobTracker.API.Interfaces;
using JobTracker.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace JobTracker.API.Controllers
{
    [ApiController]
    [Route("api/feedback")]
    public class QueriesAndFeedbackController : ControllerBase
    {
        private readonly IQueriesAndFeedbacksServices _feedbackService;

        public QueriesAndFeedbackController(IQueriesAndFeedbacksServices feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [Authorize]
        [HttpPost("submit")]
        public async Task<ActionResult<CommonResponsedto>> SubmitFeedback([FromBody] QueriesAndFeedbackdto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new CommonResponsedto
                {
                    Success = false,
                    Message = "Invalid token"
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Comment))
            {
                return BadRequest(new CommonResponsedto
                {
                    Success = false,
                    Message = "Comment is required"
                });
            }

            await _feedbackService.SubmitFeedbackAsync(userId, dto);

            return Ok(new CommonResponsedto
            {
                Success = true,
                Message = "Feedback submitted successfully"
            });
        }


    }
}
