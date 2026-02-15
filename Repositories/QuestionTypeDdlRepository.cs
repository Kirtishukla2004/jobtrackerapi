using System.Data;
using JobTracker.API.Data;
using JobTracker.API.DTOs;
using JobTracker.API.Repositories;
using Microsoft.Data.SqlClient;

namespace JobTracker.API.Repositories.Implementations
{
    public class QuestionTypeDdlRepository : IQuestionTypeDdlRepository
    {
        private readonly DBHelper _dbHelper;

        public QuestionTypeDdlRepository(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<List<QuestionTypeDdldto>> GetQuestionTypesByCategoryAsync(int categoryId)
        {
            var parameters = new[]
            {
                new SqlParameter("@CategoryId", categoryId)
            };

            var result = await _dbHelper.ExecuteStoredProcedureAsync<QuestionTypeDdldto>( "sp_GetQuestionTypesByCategoryDDL",parameters);

            return result ?? new List<QuestionTypeDdldto>();
        }
    }
}
