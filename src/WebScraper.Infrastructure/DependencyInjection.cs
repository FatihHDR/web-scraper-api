using Microsoft.Extensions.DependencyInjection;
using WebScraper.Domain.Interfaces;
using WebScraper.Infrastructure.Services;

namespace WebScraper.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IBrowserService, PuppeteerBrowserService>();
        return services;
    }
}
