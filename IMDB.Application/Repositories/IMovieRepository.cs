using IMDB.Application.Models;

namespace IMDB.Application.Repositories;

public interface IMovieRepository
{
    Task<bool> CreateAsync(Movie movie,CancellationToken token);
    Task<Movie?> GetByIdAsync(Guid id,CancellationToken token);
    Task<Movie?> GetBySlugAsync(string slug,CancellationToken token);
    Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token);
    Task<bool> UpdateAsync(Movie movie,CancellationToken token);
    Task<bool> DeleteAsync(Guid id,CancellationToken token);
    Task<bool> ExistsByIdAsync(Guid id,CancellationToken token);
}