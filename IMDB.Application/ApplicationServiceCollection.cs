using IMDB.Application.Database;
using IMDB.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB.Application;

public static class ApplicationServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;
    }
    public static IServiceCollection AddDatabse(this IServiceCollection services , string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => 
                        new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}