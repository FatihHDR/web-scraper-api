using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Application.Commands;

/// <summary>
/// Base interface for all CLI commands.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// The command name used to invoke this command.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// A brief description of the command.
    /// </summary>
    string Description { get; }
    
    /// <summary>
    /// Usage syntax for the command.
    /// </summary>
    string Usage { get; }
    
    /// <summary>
    /// Executes the command with the given arguments.
    /// </summary>
    Task<ScraperResult> ExecuteAsync(IBrowserService browser, string[] args);
}
