using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class TypeCommand : ICommand
{
    public string Name => "type";
    public string Description => "Type text into an input element";
    public string Usage => "type <css-selector> <text>";

    public async Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        if (args.Length < 2)
        {
            return ScraperResult.Error("Usage: type <css-selector> <text>");
        }

        var selector = args[0];
        var text = string.Join(" ", args.Skip(1));
        return await browser.TypeTextAsync(selector, text);
    }
}
