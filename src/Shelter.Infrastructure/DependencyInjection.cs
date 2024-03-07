using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shelter.Application.Abstractions.Caching;
using Shelter.Application.Abstractions.Clock;
using Shelter.Application.Abstractions.Email;
using Shelter.Infrastructure.Caching;
using Shelter.Infrastructure.Clock;
using Shelter.Infrastructure.Email;

namespace Shelter.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(
            configuration.GetSection("EmailSettings"));

        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();

        AddCaching(services, configuration);

        return services;
    }

    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("Cache") ??
            throw new ArgumentNullException(nameof(configuration));

        services.AddStackExchangeRedisCache(options => options.Configuration = connection);

        services.AddSingleton<ICacheService, CacheService>();
    }
}
