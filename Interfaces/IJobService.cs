using JobTracker.API.DTOs;

namespace JobTracker.API.Interfaces
{
    public interface IJobService
    {
        Task<PagedDatadto<JobDatadto>> GetJobsByUserAsync(int userId, JobFilterdto filters);
        Task<JobDatadto> AddJobAsync(JobDatadto job, int userId);
        Task<List<JobStatusdto>> GetJobStatusesAsync();
        Task<bool> DeleteJobAsync(int jobId, int userId);
    }
}
