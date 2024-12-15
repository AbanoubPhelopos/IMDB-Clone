using System.Data;
using Npgsql;

namespace IMDB.Application.Database;

public interface IDbConnectionFactory
{
    Task<IDbConnection> GetConnectionAsync(CancellationToken token=default);
}

public class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory
{

    public async Task<IDbConnection> GetConnectionAsync(CancellationToken token=default)
    {
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(token);
        return connection;
    }
}