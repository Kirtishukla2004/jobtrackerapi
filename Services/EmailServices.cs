using System.Net;
using System.Net.Mail;
using System.Text;
using JobTracker.API.Interfaces;
using static System.Net.WebRequestMethods;

public class EmailServices : IEmailServices
{
    private readonly IConfiguration _config;
    private readonly HttpClient _http;
  
    public EmailServices(HttpClient http, IConfiguration config)
    {
        _config = config;
        _http = http;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var client = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _config["Email:Username"],
                _config["EMAIL_PASSWORD"]
            )
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_config["Email:From"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        await client.SendMailAsync(mail);
    }
    public async Task SendFeedbackEmailAsync(string subject, string body)
    {
        var client = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(
               _config["Email:Username"],
               _config["Email:Password"]
           )
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_config["Email:From"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(_config["Feedback:ToEmail"]); 

        await client.SendMailAsync(mail);
    }
}

