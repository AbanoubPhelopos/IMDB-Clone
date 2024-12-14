namespace IMDB.Contracts.Responses;
public class MoviesResponse
{
    public required IEnumerable<MoviesResponse> Movies { get; init; } = Enumerable.Empty<MoviesResponse>();
}