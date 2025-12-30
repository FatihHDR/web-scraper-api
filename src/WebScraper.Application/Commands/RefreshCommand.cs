using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class RefreshCommand : ICommand
{
    public string Name => "refresh";
    public string Description => "Reload the current page";
    public string Usage => "refresh";

    public async Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        return await browser.RefreshAsync();
    }
}
