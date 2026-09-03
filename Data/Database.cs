using Microsoft.Data.Sqlite;

namespace CarStockApi.Data
{
    public class Database
    {
        private readonly string _connectionString;

        public Database(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=carstock.db";
        }

        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        public void Initialize()
        {
            using var connection = CreateConnection();

            connection.Open();

            using var pragmaCommand = connection.CreateCommand();
            pragmaCommand.CommandText = "PRAGMA foreign_keys = ON;";
            pragmaCommand.ExecuteNonQuery();

            var sql = File.ReadAllText("Data/schema.sql");

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();

            SeedDealers(connection);
        }

        private static void SeedDealers(SqliteConnection connection)
        {
            var dealer1PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123");

            var dealer2PasswordHash = BCrypt.Net.BCrypt.HashPassword("password456");

            const string sql = """
                INSERT OR IGNORE INTO Dealers
                    (DealerId, Username, PasswordHash)
                VALUES
                    (1, 'dealer1', @Dealer1PasswordHash);

                INSERT OR IGNORE INTO Dealers
                    (DealerId, Username, PasswordHash)
                VALUES
                    (2, 'dealer2', @Dealer2PasswordHash);
                """;

            using var command = connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "@Dealer1PasswordHash",
                dealer1PasswordHash);

            command.Parameters.AddWithValue(
                "@Dealer2PasswordHash",
                dealer2PasswordHash);

            command.ExecuteNonQuery();
        }
    }
}
