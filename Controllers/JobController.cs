using System.Security.Claims;
using JobTracker.API.DTOs;
using JobTracker.API.Interfaces;
using JobTracker.API.Models;
using JobTracker.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace JobTracker.API.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [Authorize]
        [HttpPost("getjobs")]
        public async Task<ActionResult<JobDashboarddto>> GetJobs([FromBody] JobFilterdto filters)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _jobService.GetJobsByUserAsync(userId, filters);
            return Ok(result);
        }



        [HttpPost("addjob")]
        public async Task<ActionResult<JobDatadto>> Addjob([FromBody] JobDatadto jobDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            jobDto.UserId = userId;

            var createdJob = await _jobService.AddJobAsync(jobDto, userId);

            return Ok(createdJob);
        }
        [Authorize]
        [HttpGet("statuses")]
        public async Task<ActionResult<List<JobStatusdto>>> GetJobStatuses()
        {
            var statuses = await _jobService.GetJobStatusesAsync();
            return Ok(statuses);
        }
        [HttpDelete("{jobId}")]
        public async Task<IActionResult> DeleteJob(int jobId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var deleted = await _jobService.DeleteJobAsync(jobId, userId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

    }

}



