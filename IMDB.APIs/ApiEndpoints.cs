namespace IMDB.APIs;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    public static class Movie
    {
        private const string Base = $"{ApiBase}/movies";
        public const string Create = Base;
        public const string Get = $"{Base}/{{Id:Guid}}";
        public const string GetAll = Base;
    }
}