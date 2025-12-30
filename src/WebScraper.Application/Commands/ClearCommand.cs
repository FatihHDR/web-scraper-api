using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class ClearCommand : ICommand
{
    public string Name => "clear";
    public string Description => "Clear the console screen";
    public string Usage => "clear";

    public Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        Console.Clear();
        return Task.FromResult(ScraperResult.Ok());
    }
}
