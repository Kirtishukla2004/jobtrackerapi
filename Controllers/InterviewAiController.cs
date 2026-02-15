using JobTracker.API.DTOs;
using JobTracker.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/interview")]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewAiService _aiService;

        public InterviewController(IInterviewAiService aiService)
        {
            _aiService = aiService;
        }
        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] InterviewAidto request)
        {
            var result = await _aiService.GenerateInterviewAsync(request);
            return Ok(result);
        }

    }



}
