using System.Text.RegularExpressions;

namespace IMDB.Application.Models;

public partial class Movie
{
    public required Guid Id { get; init; }
    public required string Title { get; set; }
    public string Slug => GenerateSlug();
    public required int YearOfRelease { get; set; }
    public required List<string> Genres { get; set; } = new();
    private string GenerateSlug()
    {
        var slugTitle = SlugRegex().Replace(Title, string.Empty)
                        .ToLower().Replace(" ", "-");
        return $"{slugTitle}-{YearOfRelease}"; 
    }

    [GeneratedRegex("[^a-z0-9A-Z _-]", RegexOptions.NonBacktracking,5)]
    private static partial Regex SlugRegex();

}
