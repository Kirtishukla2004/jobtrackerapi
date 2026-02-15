namespace JobTracker.API.Interfaces
{
    public interface  IEmailServices
    {
        Task SendAsync(string to, string subject, string body);
        Task SendFeedbackEmailAsync(string subject, string body);
    }
}
