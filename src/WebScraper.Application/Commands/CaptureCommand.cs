using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class CaptureCommand : ICommand
{
    public string Name => "capture";
    public string Description => "Capture HTML content using a CSS selector";
    public string Usage => "capture <css-selector>";

    public async Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        if (args.Length == 0)
        {
            return ScraperResult.Error("Usage: capture <css-selector>");
        }

        var selector = string.Join(" ", args);
        return await browser.CaptureElementAsync(selector);
    }
}
