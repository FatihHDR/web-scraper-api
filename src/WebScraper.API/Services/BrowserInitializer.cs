using WebScraper.Domain.Interfaces;

namespace WebScraper.API.Services;

public class BrowserInitializer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BrowserInitializer> _logger;

    public BrowserInitializer(IServiceProvider serviceProvider, ILogger<BrowserInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Initializing browser service...");
            using var scope = _serviceProvider.CreateScope();
            var browserService = scope.ServiceProvider.GetRequiredService<IBrowserService>();
            
            await browserService.InitializeAsync();
             _logger.LogInformation("Browser service initialized successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize browser service.");
        }
    }
}
