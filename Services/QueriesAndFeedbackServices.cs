using JobTracker.API.Interfaces;
using JobTracker.API.DTOs;

public class QueriesAndFeedbackServices : IQueriesAndFeedbacksServices
{
    private readonly IQueriesAndFeedbackRepository _feedbackRepo;
    private readonly IEmailServices _emailService;

    public QueriesAndFeedbackServices(
        IQueriesAndFeedbackRepository feedbackRepo,
        IEmailServices emailService)
    {
        _feedbackRepo = feedbackRepo;
        _emailService = emailService;
    }

    public async Task SubmitFeedbackAsync(int userid, QueriesAndFeedbackdto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Comment))
            return;

        await _feedbackRepo.SaveFeedbackAsync(userid, dto.Comment);

        var body = $@"
            <h3>New Feedback Received</h3>
            <p><strong>User ID:</strong> {userid}</p>
            <p><strong>Message:</strong></p>
            <p>{dto.Comment}</p>
        ";

        try
        {
            await _emailService.SendFeedbackEmailAsync(
                "New Feedback / Query",
                body
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine("Feedback email failed:");
            Console.WriteLine(ex);
        }
    }
}
