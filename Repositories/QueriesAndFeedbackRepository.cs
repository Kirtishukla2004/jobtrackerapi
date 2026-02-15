using JobTracker.API.Data;
using JobTracker.API.Interfaces;
using Microsoft.Data.SqlClient;

public class QueriesAndFeedbackRepository : IQueriesAndFeedbackRepository
{
    private readonly DBHelper _dbHelper;

    public QueriesAndFeedbackRepository(DBHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    public async Task SaveFeedbackAsync(int userid,string Comment)
    {
        var parameters = new[]
        {
            new SqlParameter("@Userid", userid),
            new SqlParameter("@Comment", Comment)
        };

        await _dbHelper.ExecuteStoredProcedureAsync<int>("sp_SaveFeedback",parameters
        );
    }
}
