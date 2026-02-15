using JobTracker.API.DTOs;

namespace JobTracker.API.Services
{
    public interface IInterviewQuestionOptionDdlService
    {
        Task<List<CategoryDdldto>> GetCategoryDdlAsync();

        Task<List<QuestionTypeDdldto>> GetQuestionTypesByCategoryAsync(int categoryId);
    }
}
