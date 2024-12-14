namespace IMDB.Contracts.Responses;
public class MoviesResponse
{
    public required IEnumerable<MovieResponse> Movies { get; init; } = Enumerable.Empty<MovieResponse>();
}