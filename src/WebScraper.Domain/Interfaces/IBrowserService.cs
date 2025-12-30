using WebScraper.Domain.Entities;

namespace WebScraper.Domain.Interfaces;

/// <summary>
/// Interface for browser automation services.
/// </summary>
public interface IBrowserService : IAsyncDisposable
{
    /// <summary>
    /// Gets the current browser state.
    /// </summary>
    BrowserState State { get; }
    
    /// <summary>
    /// Initializes the browser instance.
    /// </summary>
    Task InitializeAsync();
    
    /// <summary>
    /// Navigates to the specified URL.
    /// </summary>
    Task<ScraperResult> NavigateAsync(string url);
    
    /// <summary>
    /// Gets the full HTML content of the current page.
    /// </summary>
    Task<ScraperResult> GetPageContentAsync();
    
    /// <summary>
    /// Captures HTML content of elements matching the CSS selector.
    /// </summary>
    Task<ScraperResult> CaptureElementAsync(string selector);
    
    /// <summary>
    /// Clicks on an element matching the CSS selector.
    /// </summary>
    Task<ScraperResult> ClickElementAsync(string selector);
    
    /// <summary>
    /// Types text into an element matching the CSS selector.
    /// </summary>
    Task<ScraperResult> TypeTextAsync(string selector, string text);
    
    /// <summary>
    /// Takes a screenshot of the current page.
    /// </summary>
    Task<ScraperResult> TakeScreenshotAsync(string? path = null);
    
    /// <summary>
    /// Waits for the specified duration.
    /// </summary>
    Task<ScraperResult> WaitAsync(int milliseconds);
    
    /// <summary>
    /// Navigates back in browser history.
    /// </summary>
    Task<ScraperResult> GoBackAsync();
    
    /// <summary>
    /// Navigates forward in browser history.
    /// </summary>
    Task<ScraperResult> GoForwardAsync();
    
    /// <summary>
    /// Refreshes the current page.
    /// </summary>
    Task<ScraperResult> RefreshAsync();
    
    /// <summary>
    /// Scrolls the page in the specified direction.
    /// </summary>
    Task<ScraperResult> ScrollAsync(string direction, int pixels = 300);
    
    /// <summary>
    /// Evaluates JavaScript code on the page.
    /// </summary>
    Task<ScraperResult> EvaluateScriptAsync(string script);
}
