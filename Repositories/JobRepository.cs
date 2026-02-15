using System.Data;
using JobTracker.API.Data;
using JobTracker.API.DTOs;
using JobTracker.API.Interfaces;
using JobTracker.API.Models;
using Microsoft.Data.SqlClient;

namespace JobTracker.API.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly DBHelper _dbHelper;

        public JobRepository(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<JobDatadto> AddJobAsync(JobDatadto job, int userId)
        {
            var parameters = new[]
            {
            new SqlParameter("@UserId", userId),
            new SqlParameter("@CompanyName", job.CompanyName),
            new SqlParameter("@Role", job.Role),
            new SqlParameter("@StatusId", job.StatusId),
            new SqlParameter("@RoundNote", job.RoundNote ?? (object)DBNull.Value),
            new SqlParameter("@AppliedDate", job.AppliedDate),
            new SqlParameter("@PlatformAppliedOn",job.PlatformAppliedOn ?? (object)DBNull.Value),
            new SqlParameter("@JobDescription", job.JobDescription ?? (object)DBNull.Value),
            new SqlParameter("@ContactPerson",job.ContactPerson ?? (object)DBNull.Value),
            new SqlParameter("@ContactInfo",job.ContactInfo ?? (object)DBNull.Value),
            };
           
            var result = await _dbHelper.ExecuteStoredProcedureAsync<JobDatadto>("sp_AddJob",parameters);
            return result.First();
        }
        public async Task<PagedDatadto<JobDatadto>> GetJobsByUserAsync(int userId,JobFilterdto filters)
        {
            var parameters = new[]
            {
            new SqlParameter("@UserId", userId),
            new SqlParameter("@StatusId", (object?)filters.StatusId ?? DBNull.Value),
            new SqlParameter("@FromDate", (object?)filters.FromDate?.ToDateTime(TimeOnly.MinValue) ?? DBNull.Value),
            new SqlParameter("@ToDate", (object?)filters.ToDate?.ToDateTime(TimeOnly.MinValue) ?? DBNull.Value),
            new SqlParameter("@Search", (object?)filters.Search ?? DBNull.Value),
            new SqlParameter("@Page", filters.Page),
            new SqlParameter("@PageSize", filters.PageSize)};

            return await _dbHelper.ExecutePagedStoredProcedureAsync<JobDatadto>("sp_GetJobsByUser",parameters);
        }

        public async Task<List<JobStatusdto>> GetJobStatusesAsync()
        {
            var result = new List<JobStatusdto>();

            var reader = await _dbHelper.ExecuteReaderAsync(
                commandText: @"SELECT StatusId, DisplayName FROM JobStatus WHERE IsActive = 1 ORDER BY DisplayName",
                commandType: CommandType.Text
            );

            while (await reader.ReadAsync())
            {
                result.Add(new JobStatusdto
                {
                    StatusId = reader.GetInt32(reader.GetOrdinal("StatusId")),
                    DisplayName = reader.GetString(reader.GetOrdinal("DisplayName"))
                });
            }

            return result;
        }

        public async Task<bool> DeleteJobAsync(int jobId, int userId)
        {
            var parameters = new[]
            {
        new SqlParameter("@JobId", SqlDbType.Int) { Value = jobId },
        new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
    };

            var rowsAffected = await _dbHelper.ExecuteNonQueryAsync(
                "sp_DeleteJob",
                parameters
            );

            return rowsAffected > 0;
        }




    }

}
