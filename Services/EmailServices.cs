using System.Text;
using System.Text.Json;
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
        var apiKey = Environment.GetEnvironmentVariable("RESEND_API_KEY")
            ?? throw new InvalidOperationException("RESEND_API_KEY missing");

        var fromEmail = Environment.GetEnvironmentVariable("EMAIL_FROM")
            ?? throw new InvalidOperationException("EMAIL_FROM missing");

        var payload = new
        {
            from = $"JobTracker <{fromEmail}>",
            to = new[] { to },
            subject = subject,
            html = body
        };

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.resend.com/emails"
        );

        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _http.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine("RESEND STATUS: " + response.StatusCode);
        Console.WriteLine("RESEND RESPONSE: " + responseBody);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Resend failed: " + responseBody);
        }
    }

    public async Task SendFeedbackEmailAsync(string subject, string body)
    {
        var feedbackToEmail = Environment.GetEnvironmentVariable("Feedback_To_Email")
            ?? throw new InvalidOperationException("Feedback_To_Email missing");

        await SendAsync(feedbackToEmail, subject, body);
    }
}
