using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class NavigateCommand : ICommand
{
    public string Name => "navigate";
    public string Description => "Navigate to a URL";
    public string Usage => "navigate <url>";

    public async Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        if (args.Length == 0)
        {
            return ScraperResult.Error("Usage: navigate <url>");
        }

        var url = args[0];
        if (!url.StartsWith("http://") && !url.StartsWith("https://"))
        {
            url = "https://" + url;
        }

        return await browser.NavigateAsync(url);
    }
}
