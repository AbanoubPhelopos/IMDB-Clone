using Dapper;
using IMDB.Application.Database;
using IMDB.Application.Models;

namespace IMDB.Application.Repositories;

public class MovieRepository(IDbConnectionFactory dbFactory) : IMovieRepository
{
    public async Task<bool> CreateAsync(Movie movie,CancellationToken token)
    {
        using var connection = await dbFactory.GetConnectionAsync(token);
        using var transaction = connection.BeginTransaction();
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                      insert into movies (id,slug,title,yearofrelease)
                                      values (@Id,@Slug,@Title,@YearOfRelease)
                                      """, movie,cancellationToken:token));
        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                                                                    insert into genres (movieId, name)
                                                                    values (@Id, @Name)
                                                                    """, new { Id = movie.Id, Name = genre },
                    cancellationToken: token));
            }
        }
        transaction.Commit();

        return result > 0;
    }

    public async Task<Movie?> GetByIdAsync(Guid id,CancellationToken token)
    {
        using var connection = await dbFactory.GetConnectionAsync(token);
        var movie = connection.QuerySingleOrDefault<Movie>(new CommandDefinition("""
            select * from movies where id = @id
            """,new { id },cancellationToken:token));
        if (movie is null)
        {
            return null;
        }
        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  select name from genres where movieId = @id
                                  """,new { id },cancellationToken:token));
        foreach (var genre in genres)
            movie.Genres.Add(genre);
        return movie;
    }

    public async Task<Movie?> GetBySlugAsync(string slug,CancellationToken token)
    {
        using var connection = await dbFactory.GetConnectionAsync(token);
        var movie = connection.QuerySingleOrDefault<Movie>(new CommandDefinition("""
            select * from movies where slug = @slug
            """,new { slug },cancellationToken:token));
        if (movie is null)
        {
            return null;
        }
        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  select name from genres where movieId = @id
                                  """,new { id=movie.Id },cancellationToken:token));
        foreach (var genre in genres)
            movie.Genres.Add(genre);
        return movie;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token)
    {
        using var connection = await dbFactory.GetConnectionAsync(token);
        var result = await connection.QueryAsync(new CommandDefinition("""
                                                                       select m.*, string_agg(g.name , ',') as genres
                                                                       from movies m left join genres g on m.id = g.movieId
                                                                       group by id
                                                                       """,cancellationToken:token));
        return result.Select(x => new Movie
        {
            Id = x.id,
            Title = x.title,
            YearOfRelease = x.yearofrelease,
            Genres = Enumerable.ToList(x.genres.Split(','))
        });
    }

    public async Task<bool> UpdateAsync(Movie movie,CancellationToken token)
    {
        using var connection = await dbFactory.GetConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
                                                            delete from genres where movieId = @id
                                                            """, new { id = movie.Id }, cancellationToken: token));
        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                                                                insert into genres (movieId, name) values (@MovieId, @Name)
                                                                """, new { MovieId = movie.Id, Name = genre },
                cancellationToken: token));
        }

        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         update movies set slug = @Slug, title = @Title, yearofrelease = @YearOfRelease
                                                                         where id = @Id
                                                                         """, movie, cancellationToken: token));
        transaction.Commit();
        return result > 0;
        
    }

    public async Task<bool> DeleteAsync(Guid id,CancellationToken token)
    {
        using var connection = await dbFactory.GetConnectionAsync(token);
        using var transaction = connection.BeginTransaction();
        
        await connection.ExecuteAsync(new CommandDefinition("""
                                                      delete from genres where movieId = @id
                                                      """, new { id },cancellationToken:token));
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                   delete from movies where id = @id
                                                                   """, new { id },cancellationToken:token));
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id,CancellationToken token)
    { 
        using var connection = await dbFactory.GetConnectionAsync(token);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                                                                               select count(1) from movies where id = @id
                                                                               """,new { id },cancellationToken:token));
    }
}