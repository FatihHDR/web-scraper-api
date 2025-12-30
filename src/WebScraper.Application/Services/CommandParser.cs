using WebScraper.Application.Commands;

namespace WebScraper.Application.Services;

/// <summary>
/// Parses user input into commands and arguments.
/// </summary>
public class CommandParser
{
    private readonly Dictionary<string, ICommand> _commands;

    public CommandParser(IEnumerable<ICommand> commands)
    {
        _commands = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase);
        
        foreach (var command in commands)
        {
            _commands[command.Name] = command;
        }
    }

    /// <summary>
    /// Parses user input and returns the matching command with arguments.
    /// </summary>
    public (ICommand? Command, string[] Args) Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return (null, Array.Empty<string>());
        }

        input = input.Trim();

        // Try to match multi-word commands first (like "click on", "show code")
        foreach (var commandName in _commands.Keys.OrderByDescending(k => k.Length))
        {
            if (input.StartsWith(commandName, StringComparison.OrdinalIgnoreCase))
            {
                var remaining = input[commandName.Length..].Trim();
                var args = string.IsNullOrEmpty(remaining) 
                    ? Array.Empty<string>() 
                    : ParseArguments(remaining);
                return (_commands[commandName], args);
            }
        }

        return (null, Array.Empty<string>());
    }

    /// <summary>
    /// Gets all registered commands.
    /// </summary>
    public IEnumerable<ICommand> GetAllCommands() => _commands.Values;

    private static string[] ParseArguments(string input)
    {
        var args = new List<string>();
        var current = string.Empty;
        var inQuotes = false;

        foreach (var c in input)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ' ' && !inQuotes)
            {
                if (!string.IsNullOrEmpty(current))
                {
                    args.Add(current);
                    current = string.Empty;
                }
            }
            else
            {
                current += c;
            }
        }

        if (!string.IsNullOrEmpty(current))
        {
            args.Add(current);
        }

        return args.ToArray();
    }
}
