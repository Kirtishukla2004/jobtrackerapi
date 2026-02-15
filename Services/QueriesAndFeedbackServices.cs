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

    public async Task SubmitFeedbackAsync(int  userid,QueriesAndFeedbackdto dto)
    { 
        await _feedbackRepo.SaveFeedbackAsync(userid, dto.Comment);

        var body = $@"
            <h3>New Feedback Received</h3>
            <p><strong>User ID:</strong> {userid}</p>
            <p><strong>Message:</strong></p>
            <p>{dto.Comment}</p>
        ";

        await _emailService.SendFeedbackEmailAsync(
            "New Feedback / Query",
            body
        );
    }
}
