using JobTracker.API.DTOs;

public class JobDashboarddto
{
    public List<JobDatadto> Jobs { get; set; } = new();
    public int TotalCount { get; set; }

    public Dictionary<string, int> StatusCounts { get; set; } = new();
}
