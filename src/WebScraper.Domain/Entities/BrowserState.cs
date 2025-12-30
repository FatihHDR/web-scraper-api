namespace WebScraper.Domain.Entities;

/// <summary>
/// Represents the current state of the browser session.
/// </summary>
public class BrowserState
{
    public string CurrentUrl { get; set; } = string.Empty;
    public string PageTitle { get; set; } = string.Empty;
    public bool IsConnected { get; set; }
    public DateTime LastNavigationTime { get; set; }
    
    public override string ToString()
    {
        return $"[{(IsConnected ? "Connected" : "Disconnected")}] {PageTitle} - {CurrentUrl}";
    }
}
