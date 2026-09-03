using Dapper;
using CarStockApi.Models;

namespace CarStockApi.Data.Repositories;

public class CarRepository
{
    private readonly Database _database;

    public CarRepository(Database database)
    {
        _database = database;
    }

    public async Task<IEnumerable<Car>> GetAllAsync(int dealerId)
    {
        using var connection = _database.CreateConnection();

        const string sql = """
            SELECT CarId, DealerId, Make, Model, Year, Stock
            FROM Cars
            WHERE DealerId = @DealerId;
            """;

        return await connection.QueryAsync<Car>(
            sql,
             new
             {
                 DealerId = dealerId
             });
    }

    public async Task<int> AddAsync(Car car)
    {
        using var connection = _database.CreateConnection();

        const string sql = """
        INSERT INTO Cars (DealerId, Make, Model, Year, Stock)
        VALUES (@DealerId, @Make, @Model, @Year, @Stock);

        SELECT last_insert_rowid();
        """;

        return await connection.ExecuteScalarAsync<int>(sql, car);
    }

    public async Task<Car?> GetByIdAsync(int carId, int dealerId)
    {
        using var connection = _database.CreateConnection();

        const string sql = """
            SELECT CarId, DealerId, Make, Model, Year, Stock
            FROM Cars
            WHERE CarId = @CarId
            AND DealerId = @DealerId;
            """;

        return await connection.QuerySingleOrDefaultAsync<Car>(
            sql,
            new
            {
                CarId = carId,
                DealerId = dealerId
            });
    }

    public async Task<bool> DeleteAsync(int carId, int dealerId)
    {
        using var connection = _database.CreateConnection();

        const string sql = """
            DELETE FROM Cars
            WHERE CarId = @CarId
            AND DealerId = @DealerId;
            """;

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                CarId = carId,
                DealerId = dealerId
            });

        return rowsAffected > 0;
    }

    public async Task<bool> UpdateStockAsync(int carId, int stock, int dealerId)
    {
        using var connection = _database.CreateConnection();

        const string sql = """
            UPDATE Cars
            SET Stock = @Stock
            WHERE CarId = @CarId
            AND DealerId = @DealerId;
            """;

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                CarId = carId,
                Stock = stock,
                DealerId = dealerId
            });

        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Car>> SearchAsync(string make, string model, int dealerId)
    {
        using var connection = _database.CreateConnection();

        const string sql = """
            SELECT CarId, DealerId, Make, Model, Year, Stock
            FROM Cars
            WHERE Make = @Make 
            AND Model = @Model
            AND DealerId = @DealerId;
            """;

        return await connection.QueryAsync<Car>(
            sql,
            new
            {
                Make = make,
                Model = model,
                DealerId = dealerId
            });
    }
}