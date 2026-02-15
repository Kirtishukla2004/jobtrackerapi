using JobTracker.API.DTOs;

namespace JobTracker.API.Repositories
{
    public interface IQuestionTypeDdlRepository
    {
        Task<List<QuestionTypeDdldto>> GetQuestionTypesByCategoryAsync(int categoryId);
    }
}
