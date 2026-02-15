namespace JobTracker.API.DTOs
{
    public class JobDatadto
    {
        public int JobId { get; set; }
        public int UserId { get; set; }

        public string CompanyName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? RoundNote { get; set; }
        public DateTime AppliedDate { get; set; }

        public string? PlatformAppliedOn { get; set; }
        public string? JobDescription { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactInfo { get; set; }
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
    }

}
