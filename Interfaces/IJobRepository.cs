using JobTracker.API.DTOs;

namespace JobTracker.API.Interfaces
{
    public interface IJobRepository
    {
        Task<JobDatadto> AddJobAsync(JobDatadto job, int userId);
        Task<List<JobStatusdto>> GetJobStatusesAsync();
        Task<PagedDatadto<JobDatadto>> GetJobsByUserAsync(int userId, JobFilterdto filters);
        Task<bool> DeleteJobAsync(int jobId, int userId);

    }
}
