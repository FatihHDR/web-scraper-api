using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class ScreenshotCommand : ICommand
{
    public string Name => "screenshot";
    public string Description => "Take a screenshot of the current page";
    public string Usage => "screenshot [filename]";

    public async Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        var path = args.Length > 0 ? args[0] : null;
        return await browser.TakeScreenshotAsync(path);
    }
}
