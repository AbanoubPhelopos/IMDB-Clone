using Dapper;

namespace IMDB.Application.Database;

public class DbInitializer(IDbConnectionFactory connectionFactory)
{
    private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.GetConnectionAsync();
        await connection.ExecuteAsync("""
                                      create table if not exists movies(
                                          id UUID primary key,
                                          slug Text not null,
                                          title Text not null,
                                          yearofrelease integer not null);
                                      """);
        await connection.ExecuteAsync("""
                                      create unique index if not exists movies_slug_idx
                                      on movies
                                      using btree(slug);
                                      """);
    }
}