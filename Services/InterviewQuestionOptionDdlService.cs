using JobTracker.API.DTOs;
using JobTracker.API.Repositories;
using JobTracker.API.Services;

namespace JobTracker.API.Services.Implementations
{
    public class InterviewQuestionOptionDdlService
        : IInterviewQuestionOptionDdlService
    {
        private readonly IInterviewQuestionOptionDdlRepository _repository;

        public InterviewQuestionOptionDdlService(
            IInterviewQuestionOptionDdlRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CategoryDdldto>> GetCategoryDdlAsync()
        {
            return await _repository.GetCategoryDdlAsync();
        }
        public async Task<List<QuestionTypeDdldto>> GetQuestionTypesByCategoryAsync(int categoryId)
        {
            return await _repository.GetQuestionTypesByCategoryAsync(categoryId);
        }
    }
}
