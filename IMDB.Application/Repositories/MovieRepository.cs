using IMDB.Application.Models;

namespace IMDB.Application.Repositories;

public class MovieRepository:IMovieRepository
{
    private readonly List<Movie> _movies = new();
    public Task<bool> CreateAsync(Movie movie)
    {
        _movies.Add(movie);
        return Task.FromResult(true);
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        var index =_movies.FindIndex(m=>m.Id == id);
        if (index == -1)
        {
            return Task.FromResult<Movie?>(null);
        }
        return Task.FromResult<Movie?>(_movies[index]);
    }

    public Task<Movie?> GetBySlugAsync(string slug)
    {
        var index =_movies.FindIndex(m=>m.Slug == slug);
        if (index == -1)
        {
            return Task.FromResult<Movie?>(null);
        }
        return Task.FromResult<Movie?>(_movies[index]);
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return Task.FromResult(_movies.AsEnumerable());
    }

    public Task<bool> UpdateAsync(Movie movie)
    {
        var index = _movies.FindIndex(m=>m.Id == movie.Id);
        if (index == -1)
        {
            return Task.FromResult(false);
        }
        _movies[index] = movie;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var removedCount = _movies.RemoveAll(m=>m.Id == id);
        var movieremoved = removedCount > 0; 
        return Task.FromResult(movieremoved);
    }
}