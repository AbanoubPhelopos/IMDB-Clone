using IMDB.Application.Repositories;
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
}