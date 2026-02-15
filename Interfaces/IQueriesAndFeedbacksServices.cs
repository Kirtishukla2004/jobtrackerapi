using JobTracker.API.DTOs;

namespace JobTracker.API.Interfaces
{
    public interface IQueriesAndFeedbacksServices
    {
        Task SubmitFeedbackAsync(int  userid,QueriesAndFeedbackdto dto);
    }
}
