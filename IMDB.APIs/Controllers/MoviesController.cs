using IMDB.APIs.Mapping;
using IMDB.Application.Models;
using IMDB.Application.Repositories;
using IMDB.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace IMDB.APIs.Controllers;
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieRepository _movieRepository;

    public MoviesController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    [HttpPost]
    [Route(ApiEndpoints.Movie.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.MapToMovie();
        await _movieRepository.CreateAsync(movie);
        return Created($"/{ApiEndpoints.Movie.Create}/{movie.Id}", movie);
    }

    [HttpGet(ApiEndpoints.Movie.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid Id)
    {
        var movie =  await _movieRepository.GetByIdAsync(Id);
        if (movie is null)
        {
            return NotFound();
        }

        var response = movie.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Movie.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieRepository.GetAllAsync();
        
        var response = movies.MapToResponse();
        return Ok(response);
    }
    
}