using System.Data;
using JobTracker.API.Data;
using JobTracker.API.DTOs;
using Microsoft.Data.SqlClient;

namespace JobTracker.API.Repositories.Implementations
{
    public class InterviewQuestionOptionDdlRepository: IInterviewQuestionOptionDdlRepository
    {
        private readonly DBHelper _dbHelper;

        public InterviewQuestionOptionDdlRepository(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<List<CategoryDdldto>> GetCategoryDdlAsync()
        {
            var result = await _dbHelper.ExecuteStoredProcedureAsync<CategoryDdldto>("sp_GetInterviewCategoriesDDL",parameters: null);
            return result ?? new List<CategoryDdldto>();
        }
        public async Task<List<QuestionTypeDdldto>> GetQuestionTypesByCategoryAsync(int categoryId)
        {
            var parameters = new[]
            {
                new SqlParameter("@CategoryId", categoryId)
            };

            var result = await _dbHelper.ExecuteStoredProcedureAsync<QuestionTypeDdldto>(
                "sp_GetQuestionTypesByCategoryDDL",
                parameters
            );

            return result ?? new List<QuestionTypeDdldto>();
        }
    }
}
