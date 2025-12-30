using Microsoft.Extensions.DependencyInjection;
using WebScraper.Application.Commands;
using WebScraper.Application.Services;
using WebScraper.CLI.Helpers;
using WebScraper.Domain.Interfaces;
using WebScraper.Infrastructure;

// Configure services
var services = new ServiceCollection();
services.AddInfrastructure();

// Register commands
services.AddSingleton<ICommand, NavigateCommand>();
services.AddSingleton<ICommand, ShowCodeCommand>();
services.AddSingleton<ICommand, CaptureCommand>();
services.AddSingleton<ICommand, ClickCommand>();
services.AddSingleton<ICommand, TypeCommand>();
services.AddSingleton<ICommand, ScreenshotCommand>();
services.AddSingleton<ICommand, WaitCommand>();
services.AddSingleton<ICommand, BackCommand>();
services.AddSingleton<ICommand, ForwardCommand>();
services.AddSingleton<ICommand, RefreshCommand>();
services.AddSingleton<ICommand, ScrollCommand>();
services.AddSingleton<ICommand, EvalCommand>();
services.AddSingleton<ICommand, ClearCommand>();
services.AddSingleton<ICommand, ExitCommand>();

// Build service provider
var serviceProvider = services.BuildServiceProvider();
var registeredCommands = serviceProvider.GetServices<ICommand>().ToList();

// Add help command with reference to all commands
var helpCommand = new HelpCommand(registeredCommands);
registeredCommands.Add(helpCommand);

// Create command parser
var parser = new CommandParser(registeredCommands);

// Get browser service
var browserService = serviceProvider.GetRequiredService<IBrowserService>();

// Setup cancellation
var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

try
{
    // Show header
    ConsoleHelper.WriteHeader();

    // Initialize browser
    await browserService.InitializeAsync();
    Console.WriteLine();

    // Main REPL loop
    while (!cts.Token.IsCancellationRequested)
    {
        ConsoleHelper.WritePrompt(browserService.State.CurrentUrl);
        
        var input = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(input))
        {
            continue;
        }

        var (parsedCommand, args) = parser.Parse(input);

        if (parsedCommand == null)
        {
            ConsoleHelper.WriteError($"Unknown command: '{input.Split(' ')[0]}'. Type 'help' for available commands.");
            continue;
        }

        // Handle exit command
        if (parsedCommand.Name == "exit")
        {
            ConsoleHelper.WriteSuccess("Goodbye!");
            break;
        }

        try
        {
            var result = await parsedCommand.ExecuteAsync(browserService, args);

            if (result.Success)
            {
                if (!string.IsNullOrEmpty(result.Message))
                {
                    ConsoleHelper.WriteSuccess(result.Message);
                }

                if (!string.IsNullOrEmpty(result.Data))
                {
                    // Check if data looks like HTML
                    if (result.Data.TrimStart().StartsWith('<'))
                    {
                        ConsoleHelper.WriteHtml(result.Data);
                    }
                    else
                    {
                        ConsoleHelper.WriteData(result.Data);
                    }
                }
            }
            else
            {
                ConsoleHelper.WriteError(result.ErrorMessage ?? "Unknown error");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Command failed: {ex.Message}");
        }

        Console.WriteLine();
    }
}
catch (Exception ex)
{
    ConsoleHelper.WriteError($"Fatal error: {ex.Message}");
}
finally
{
    ConsoleHelper.WriteInfo("Closing browser...");
    await browserService.DisposeAsync();
}
