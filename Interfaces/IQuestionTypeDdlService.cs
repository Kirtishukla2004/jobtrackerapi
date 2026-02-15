using JobTracker.API.DTOs;

namespace JobTracker.API.Services
{
    public interface IQuestionTypeDdlService
    {
        Task<List<QuestionTypeDdldto>> GetQuestionTypesByCategoryAsync(int categoryId);
    }
}
