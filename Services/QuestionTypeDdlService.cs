using JobTracker.API.DTOs;
using JobTracker.API.Repositories;
using JobTracker.API.Services;

namespace JobTracker.API.Services.Implementations
{
    public class QuestionTypeDdlService : IQuestionTypeDdlService
    {
        private readonly IQuestionTypeDdlRepository _repository;

        public QuestionTypeDdlService(IQuestionTypeDdlRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<QuestionTypeDdldto>> GetQuestionTypesByCategoryAsync(int categoryId)
        {
            return await _repository.GetQuestionTypesByCategoryAsync(categoryId);
        }
    }
}
