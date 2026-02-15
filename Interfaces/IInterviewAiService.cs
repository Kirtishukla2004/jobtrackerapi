using JobTracker.API.DTOs;

namespace JobTracker.API.Interfaces
{
    public interface IInterviewAiService
    {
        Task<InterviewAiResponsedto> GenerateInterviewAsync(InterviewAidto request);
    }
}
