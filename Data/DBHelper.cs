using System.Data;
using JobTracker.API.DTOs;
using Microsoft.Data.SqlClient;

namespace JobTracker.API.Data
{
    public class DBHelper
    {
        private readonly IConfiguration _config;

        public DBHelper(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName,SqlParameter[]? parameters = null)where T : new()
        {
            await using var connection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            await using var command = new SqlCommand(storedProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            await using var reader = await command.ExecuteReaderAsync();

            return await SqlHelper.MapToListAsync<T>(reader);
        }
        public async Task<SqlDataReader> ExecuteReaderAsync(string commandText,SqlParameter[]? parameters = null,CommandType commandType = CommandType.StoredProcedure)
        {
            var connection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            var command = new SqlCommand(commandText, connection)
            {
                CommandType = commandType
            };

            if (parameters != null && parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            return await command.ExecuteReaderAsync(
                CommandBehavior.CloseConnection);
        }
        public async Task<int> ExecuteNonQueryAsync(string storedProcedureName, SqlParameter[]? parameters = null)
        {
            await using var connection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            await using var command = new SqlCommand(storedProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            return await command.ExecuteNonQueryAsync();
        }
        public async Task<PagedDatadto<T>> ExecutePagedStoredProcedureAsync<T>(string storedProcedureName,SqlParameter[] parameters)where T : new()
        {
            var result = new PagedDatadto<T>();

            await using var connection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            await using var command = new SqlCommand(storedProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddRange(parameters);

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                result.TotalCount = reader.GetInt32(0);
            }

            await reader.NextResultAsync();

            result.Items = await SqlHelper.MapToListAsync<T>(reader);

            return result;
        }

    }
}
