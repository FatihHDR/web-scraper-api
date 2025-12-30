using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class WaitCommand : ICommand
{
    public string Name => "wait";
    public string Description => "Wait for specified milliseconds";
    public string Usage => "wait <milliseconds>";

    public async Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        if (args.Length == 0 || !int.TryParse(args[0], out var ms))
        {
            return ScraperResult.Error("Usage: wait <milliseconds>");
        }

        return await browser.WaitAsync(ms);
    }
}
