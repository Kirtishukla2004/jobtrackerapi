using JobTracker.API.DTOs;

namespace JobTracker.API.Repositories
{
    public interface IInterviewQuestionOptionDdlRepository
    {
        Task<List<CategoryDdldto>> GetCategoryDdlAsync();
        Task<List<QuestionTypeDdldto>> GetQuestionTypesByCategoryAsync(int categoryId);
    }
}
