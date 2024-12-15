using FluentValidation;
using IMDB.Application.Models;
using IMDB.Application.Repositories;
using IMDB.Application.Services;

namespace IMDB.Application.Validators;

public class MovieValidator : AbstractValidator<Movie>,IApplicationMarker
{
    private readonly IMovieRepository _movieRepository;
    public MovieValidator(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
        RuleFor(m => m.Title)
            .NotEmpty()
            .WithMessage("Title cannot be empty");
        
        RuleFor(m=>m.Genres)
            .NotEmpty()
            .WithMessage("Genre cannot be empty, at least add one genre");
        
        RuleFor(m=>m.YearOfRelease)
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("Year must be less than or equal to the current year");

        RuleFor(m => m.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("This movie already in the system");
    }

    private async Task<bool> ValidateSlug(Movie movie,string slug,CancellationToken token=default)
    {
        var existingMovie = await _movieRepository.GetBySlugAsync(slug, token);
        if (existingMovie is not null)
        {
            return existingMovie.Id == movie.Id;
        }
        return existingMovie is null;
    }
}