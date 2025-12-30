namespace WebScraper.Domain.Entities;

/// <summary>
/// Represents the result of a scraper operation.
/// </summary>
public class ScraperResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Data { get; set; }
    public string? ErrorMessage { get; set; }
    
    public static ScraperResult Ok(string? message = null, string? data = null)
    {
        return new ScraperResult
        {
            Success = true,
            Message = message,
            Data = data
        };
    }
    
    public static ScraperResult Error(string errorMessage)
    {
        return new ScraperResult
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}
