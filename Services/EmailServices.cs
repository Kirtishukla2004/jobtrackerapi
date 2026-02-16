using System.Net;
using System.Net.Mail;
using JobTracker.API.Interfaces;

public class EmailServices : IEmailServices
{
    private readonly HttpClient _http;

    public EmailServices(HttpClient http)
    {
        _http = http;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var fromEmail =Environment.GetEnvironmentVariable("Email_from")
            ?? throw new InvalidOperationException("Email_from env variable missing");

        var emailPassword =
            Environment.GetEnvironmentVariable("Email_JobTrackerPassowrd")
            ?? throw new InvalidOperationException("Email_JobTrackerPassword env variable missing");

        using var client = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(fromEmail, emailPassword)
        };

        using var mail = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        await client.SendMailAsync(mail);
    }

    public async Task SendFeedbackEmailAsync(string subject, string body)
    {
        var fromEmail =
            Environment.GetEnvironmentVariable("Email_from")
            ?? throw new InvalidOperationException("Email_from env variable missing");

        var emailPassword =
            Environment.GetEnvironmentVariable("Email_JobTrackerPassword")
            ?? throw new InvalidOperationException("Email_JobTrackerPassword env variable missing");

        var feedbackToEmail =
            Environment.GetEnvironmentVariable("Feedback_To_Email")
            ?? throw new InvalidOperationException("Feedback_To_Email env variable missing");

        using var client = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(fromEmail, emailPassword)
        };

        using var mail = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(feedbackToEmail);

        await client.SendMailAsync(mail);
    }
}
