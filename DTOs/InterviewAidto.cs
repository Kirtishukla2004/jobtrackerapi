namespace JobTracker.API.DTOs
{
    public class InterviewAidto
    {
        public string JobDescription { get; set; } = string.Empty;
        public string? Difficulty { get; set; }
        public string? QuestionType { get; set; }
        public string? Category { get; set; }
        public int MinutesPerQuestion { get; set; }
        public int? QuestionLimit { get; set; }
    }
}
