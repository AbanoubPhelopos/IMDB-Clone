using FluentValidation;
using IMDB.Application.Models;
using IMDB.Application.Repositories;

namespace IMDB.Application.Services;

public class MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator) : IMovieService
{
    public async Task<bool> CreateAsync(Movie movie,CancellationToken token)
    {
        await movieValidator.ValidateAndThrowAsync(movie,token);
        return await movieRepository.CreateAsync(movie, token);
    }

    public Task<Movie?> GetByIdAsync(Guid id,CancellationToken token)
    {
        return movieRepository.GetByIdAsync(id,token);
    }

    public Task<Movie?> GetBySlugAsync(string slug,CancellationToken token)
    {
        return movieRepository.GetBySlugAsync(slug,token);
    }

    public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token)
    {
        return movieRepository.GetAllAsync(token);
    }

    public async Task<Movie?> UpdateAsync(Movie movie,CancellationToken token)
    {
        await movieValidator.ValidateAndThrowAsync(movie,token);
        var movieExist = await movieRepository.ExistsByIdAsync(movie.Id, token);
        if (!movieExist)
        {
            return null;
        }
        await movieRepository.UpdateAsync(movie, token);
        return movie;
    }

    public Task<bool> DeleteAsync(Guid id,CancellationToken token)
    {
        return movieRepository.DeleteAsync(id,token);
    }
}