using IMDB.Application.Models;

namespace IMDB.Application.Services;

public interface IMovieService
{
    Task<bool> CreateAsync(Movie movie,CancellationToken token);
    Task<Movie?> GetByIdAsync(Guid id,CancellationToken token);
    Task<Movie?> GetBySlugAsync(string slug,CancellationToken token);
    Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token);
    Task<Movie?> UpdateAsync(Movie movie,CancellationToken token);
    Task<bool> DeleteAsync(Guid id,CancellationToken token);
}