using JobTracker.API.Services;
using JobTracker.API.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [ApiController]
    [Route("api/interview")]
    public class InterviewQuestionOptionDdlController : ControllerBase
    {
        private readonly IInterviewQuestionOptionDdlService _interviewDdl;

        public InterviewQuestionOptionDdlController(IInterviewQuestionOptionDdlService interviewDdl)
        {
            _interviewDdl = interviewDdl;
        }

        [HttpGet("categoriesddl")]
        public async Task<IActionResult> GetCategoryDdlAsync()
        {
            var result = await _interviewDdl.GetCategoryDdlAsync();

            if (result == null || !result.Any())
            {
                return NoContent(); 
            }

            return Ok(result);
        }
        [HttpGet("questiontypesddl")]
        public async Task<IActionResult> GetQuestionTypesDdlAsync([FromQuery] int categoryId)
        {
            if (categoryId <= 0)
                return BadRequest("Invalid categoryId");

            var result = await _interviewDdl.GetQuestionTypesByCategoryAsync(categoryId);

            if (result == null || !result.Any())
                return NoContent();

            return Ok(result);
        }

    }
}
