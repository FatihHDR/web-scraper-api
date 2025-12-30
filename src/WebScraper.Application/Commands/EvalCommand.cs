using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class EvalCommand : ICommand
{
    public string Name => "eval";
    public string Description => "Execute JavaScript code on the page";
    public string Usage => "eval <javascript-code>";

    public async Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        if (args.Length == 0)
        {
            return ScraperResult.Error("Usage: eval <javascript-code>");
        }

        var script = string.Join(" ", args);
        return await browser.EvaluateScriptAsync(script);
    }
}
