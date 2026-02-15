using System.Data;
using JobTracker.API.Data;
using JobTracker.API.DTOs;
using JobTracker.API.Interfaces;

namespace JobTracker.API.Services
{
    public class JobService:IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public Task<PagedDatadto<JobDatadto>> GetJobsByUserAsync(int userId,JobFilterdto filters)
        {
            return _jobRepository.GetJobsByUserAsync(userId, filters);
        }

        public Task<JobDatadto> AddJobAsync(JobDatadto job, int userId)
        {
            return _jobRepository.AddJobAsync(job, userId);
        }
        public Task<List<JobStatusdto>> GetJobStatusesAsync()
        {
            return _jobRepository.GetJobStatusesAsync();
        }
        public Task<bool> DeleteJobAsync(int jobId, int userId)
        {
            return _jobRepository.DeleteJobAsync(jobId, userId);
        }

    }

}
