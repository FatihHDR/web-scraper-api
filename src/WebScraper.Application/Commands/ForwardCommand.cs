using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class ForwardCommand : ICommand
{
    public string Name => "forward";
    public string Description => "Navigate forward in browser history";
    public string Usage => "forward";

    public async Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        return await browser.GoForwardAsync();
    }
}
