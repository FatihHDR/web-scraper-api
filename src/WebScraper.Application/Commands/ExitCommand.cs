using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class ExitCommand : ICommand
{
    public string Name => "exit";
    public string Description => "Close the browser and exit";
    public string Usage => "exit";

    public Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        return Task.FromResult(ScraperResult.Ok("Goodbye!"));
    }
}
