using IMDB.Application.Models;
using IMDB.Application.Repositories;

namespace IMDB.Application.Services;

public class MovieService(IMovieRepository movieRepository) : IMovieService
{

    public Task<bool> CreateAsync(Movie movie)
    {
        return movieRepository.CreateAsync(movie);
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        return movieRepository.GetByIdAsync(id);
    }

    public Task<Movie?> GetBySlugAsync(string slug)
    {
        return movieRepository.GetBySlugAsync(slug);
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return movieRepository.GetAllAsync();
    }

    public async Task<Movie?> UpdateAsync(Movie movie)
    {
        var movieExist = await movieRepository.ExistsByIdAsync(movie.Id);
        if (!movieExist)
        {
            return null;
        }
        await movieRepository.UpdateAsync(movie);
        return movie;
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return movieRepository.DeleteAsync(id);
    }
}