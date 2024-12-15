using IMDB.APIs.Mapping;
using IMDB.Application.Repositories;
using IMDB.Application.Services;
using IMDB.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace IMDB.APIs.Controllers;
[ApiController]
public class MoviesController(IMovieService movieService) : ControllerBase
{
    [HttpPost(ApiEndpoints.Movie.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.MapToMovie();
        await movieService.CreateAsync(movie);
        var response = movie.MapToResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, response);
    }

    [HttpGet(ApiEndpoints.Movie.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug)
    {
        var movie =  Guid.TryParse(idOrSlug,out var id)?
            await movieService.GetByIdAsync(id)
                : await movieService.GetBySlugAsync(idOrSlug);
            
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
        var movies = await movieService.GetAllAsync();
        
        var response = movies.MapToResponse();
        return Ok(response);
    }

    [HttpPut(ApiEndpoints.Movie.Update)]
    public async Task<IActionResult> Update( [FromRoute] Guid id,
        [FromBody] UpdateMovieRequest request)
    {
        var movie = request.MapToMovie(id);
        var updatedMovie = await movieService.UpdateAsync(movie);
        if (updatedMovie is null)
        {
            return NotFound();
        }
        var response = updatedMovie.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Movie.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await movieService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
    
    
}