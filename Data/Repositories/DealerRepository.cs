using CarStockApi.Models;
using Dapper;

namespace CarStockApi.Data.Repositories
{
    public class DealerRepository
    {
        private readonly Database _database;

        public DealerRepository(Database database)
        {
            _database = database;
        }

        public async Task<Dealer?> GetByUsernameAsync(string username)
        {
            using var connection = _database.CreateConnection();

            const string sql = """
                SELECT DealerId, Username, PasswordHash
                FROM Dealers
                WHERE Username = @Username;
                """;

            return await connection.QuerySingleOrDefaultAsync<Dealer>(
                sql,
                new { Username = username });
        }
    }
}
