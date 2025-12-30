using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class ScrollCommand : ICommand
{
    public string Name => "scroll";
    public string Description => "Scroll the page up or down";
    public string Usage => "scroll <up|down> [pixels]";

    public async Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        if (args.Length == 0)
        {
            return ScraperResult.Error("Usage: scroll <up|down> [pixels]");
        }

        var direction = args[0].ToLower();
        if (direction != "up" && direction != "down")
        {
            return ScraperResult.Error("Direction must be 'up' or 'down'");
        }

        var pixels = 300;
        if (args.Length > 1 && int.TryParse(args[1], out var customPixels))
        {
            pixels = customPixels;
        }

        return await browser.ScrollAsync(direction, pixels);
    }
}
