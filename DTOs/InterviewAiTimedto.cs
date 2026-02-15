namespace JobTracker.API.DTOs
{
    public class InterviewAiTimedto
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int MinutesPerQuestion { get; set; }
    }
}
