using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class BackCommand : ICommand
{
    public string Name => "back";
    public string Description => "Navigate back in browser history";
    public string Usage => "back";

    public async Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        return await browser.GoBackAsync();
    }
}
