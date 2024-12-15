using IMDB.APIs.Mapping;
using IMDB.Application.Services;
using IMDB.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace IMDB.APIs.Controllers;
[ApiController]
public class MoviesController(IMovieService movieService) : ControllerBase
{
    [HttpPost(ApiEndpoints.Movie.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request
        ,CancellationToken token)
    {
        var movie = request.MapToMovie();
        await movieService.CreateAsync(movie,token);
        var response = movie.MapToResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, response);
    }

    [HttpGet(ApiEndpoints.Movie.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug
        ,CancellationToken token)
    {
        var movie =  Guid.TryParse(idOrSlug,out var id)?
            await movieService.GetByIdAsync(id,token)
                : await movieService.GetBySlugAsync(idOrSlug,token);
            
        if (movie is null)
        {
            return NotFound();
        }

        var response = movie.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Movie.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var movies = await movieService.GetAllAsync(token);
        
        var response = movies.MapToResponse();
        return Ok(response);
    }

    [HttpPut(ApiEndpoints.Movie.Update)]
    public async Task<IActionResult> Update( [FromRoute] Guid id,
        [FromBody] UpdateMovieRequest request,CancellationToken token)
    {
        var movie = request.MapToMovie(id);
        var updatedMovie = await movieService.UpdateAsync(movie,token);
        if (updatedMovie is null)
        {
            return NotFound();
        }
        var response = updatedMovie.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Movie.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id
        ,CancellationToken token)
    {
        var deleted = await movieService.DeleteAsync(id,token);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
    
    
}