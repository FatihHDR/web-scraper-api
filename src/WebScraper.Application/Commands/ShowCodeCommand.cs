using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class ShowCodeCommand : ICommand
{
    public string Name => "show code";
    public string Description => "Display the HTML source of the current page";
    public string Usage => "show code";

    public async Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        return await browser.GetPageContentAsync();
    }
}
