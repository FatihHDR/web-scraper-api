using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

public class HelpCommand : ICommand
{
    private readonly IEnumerable<ICommand> _commands;

    public string Name => "help";
    public string Description => "Show available commands";
    public string Usage => "help";

    public HelpCommand(IEnumerable<ICommand> commands)
    {
        _commands = commands;
    }

    public Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args)
    {
        var helpText = string.Join("\n", _commands
            .OrderBy(c => c.Name)
            .Select(c => $"  {c.Usage,-30} - {c.Description}"));

        return Task.FromResult(ScraperResult.Ok(data: $"Available commands:\n{helpText}"));
    }
}
