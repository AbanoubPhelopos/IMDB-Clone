using System.Data;
using Npgsql;

namespace IMDB.Application.Database;

public interface IDbConnectionFactory
{
    Task<IDbConnection> GetConnectionAsync();
}

public class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory
{

    public async Task<IDbConnection> GetConnectionAsync()
    {
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
    }
}