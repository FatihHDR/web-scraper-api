using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class ClickCommand : ICommand
{
    public string Name => "click on";
    public string Description => "Click on an element using a CSS selector";
    public string Usage => "click on <css-selector>";

    public async Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        if (args.Length == 0)
        {
            return ScraperResult.Error("Usage: click on <css-selector>");
        }

        var selector = string.Join(" ", args);
        return await browser.ClickElementAsync(selector);
    }
}
