using AngleSharp;
using AngleSharp.Html;
using PuppeteerSharp;
using WebScraper.Domain.Entities;
using WebScraper.Domain.Interfaces;

namespace WebScraper.Infrastructure.Services;

/// <summary>
/// Browser service implementation using PuppeteerSharp.
/// </summary>
public class PuppeteerBrowserService : IBrowserService
{
    private IBrowser? _browser;
    private IPage? _page;
    private readonly BrowserState _state = new();

    public BrowserState State => _state;

    public async Task InitializeAsync()
    {
        Console.WriteLine("🔄 Downloading Chromium browser (first run only)...");
        
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();

        Console.WriteLine("🚀 Launching browser...");
        
        _browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            Args = new[]
            {
                "--no-sandbox",
                "--disable-setuid-sandbox",
                "--disable-dev-shm-usage",
                "--disable-accelerated-2d-canvas",
                "--disable-gpu"
            }
        });

        _page = await _browser.NewPageAsync();
        
        await _page.SetViewportAsync(new ViewPortOptions
        {
            Width = 1920,
            Height = 1080
        });

        _state.IsConnected = true;
        Console.WriteLine("✅ Browser ready!");
    }

    public async Task<ScraperResult> NavigateAsync(string url)
    {
        try
        {
            EnsurePageExists();
            
            var response = await _page!.GoToAsync(url, new NavigationOptions
            {
                WaitUntil = new[] { WaitUntilNavigation.DOMContentLoaded },
                Timeout = 30000
            });

            await UpdateStateAsync();

            if (response?.Ok == true || response?.Status == System.Net.HttpStatusCode.OK)
            {
                return ScraperResult.Ok($"Navigated to {_state.CurrentUrl} (Title: {_state.PageTitle})");
            }

            return ScraperResult.Ok($"Navigated to {_state.CurrentUrl} (Status: {response?.Status})");
        }
        catch (Exception ex)
        {
            return ScraperResult.Error($"Navigation failed: {ex.Message}");
        }
    }

    public async Task<ScraperResult> GetPageContentAsync()
    {
        try
        {
            EnsurePageExists();
            
            var content = await _page!.GetContentAsync();
            var formatted = await FormatHtmlAsync(content);
            
            return ScraperResult.Ok(data: formatted);
        }
        catch (Exception ex)
        {
            return ScraperResult.Error($"Failed to get page content: {ex.Message}");
        }
    }

    public async Task<ScraperResult> CaptureElementAsync(string selector)
    {
        try
        {
            EnsurePageExists();
            
            var elements = await _page!.QuerySelectorAllAsync(selector);
            
            if (elements.Length == 0)
            {
                return ScraperResult.Error($"No elements found matching '{selector}'");
            }

            var results = new List<string>();
            foreach (var element in elements)
            {
                var outerHtml = await element.EvaluateFunctionAsync<string>("el => el.outerHTML");
                var formatted = await FormatHtmlAsync(outerHtml);
                results.Add(formatted);
            }

            return ScraperResult.Ok(
                message: $"Found {elements.Length} element(s)",
                data: string.Join("\n\n---\n\n", results)
            );
        }
        catch (Exception ex)
        {
            return ScraperResult.Error($"Capture failed: {ex.Message}");
        }
    }

    public async Task<ScraperResult> ClickElementAsync(string selector)
    {
        try
        {
            EnsurePageExists();
            
            await _page!.ClickAsync(selector);
            await Task.Delay(500); // Wait for any navigation/updates
            await UpdateStateAsync();
            
            return ScraperResult.Ok($"Clicked on element: {selector}");
        }
        catch (Exception ex)
        {
            return ScraperResult.Error($"Click failed: {ex.Message}");
        }
    }

    public async Task<ScraperResult> TypeTextAsync(string selector, string text)
    {
        try
        {
            EnsurePageExists();
            
            await _page!.TypeAsync(selector, text);
            
            return ScraperResult.Ok($"Typed \"{text}\" into {selector}");
        }
        catch (Exception ex)
        {
            return ScraperResult.Error($"Type failed: {ex.Message}");
        }
    }

    public async Task<ScraperResult> TakeScreenshotAsync(string? path = null)
    {
        try
        {
            EnsurePageExists();
            
            path ??= $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            
            await _page!.ScreenshotAsync(path, new ScreenshotOptions
            {
                FullPage = true,
                Type = ScreenshotType.Png
            });
            
            var fullPath = Path.GetFullPath(path);
            return ScraperResult.Ok($"Screenshot saved to {fullPath}");
        }
        catch (Exception ex)
        {
            return ScraperResult.Error($"Screenshot failed: {ex.Message}");
        }
    }

    public async Task<ScraperResult> WaitAsync(int milliseconds)
    {
        await Task.Delay(milliseconds);
        return ScraperResult.Ok($"Waited {milliseconds}ms");
    }

    public async Task<ScraperResult> GoBackAsync()
    {
        try
        {
            EnsurePageExists();
            
            await _page!.GoBackAsync();
            await UpdateStateAsync();
            
            return ScraperResult.Ok($"Navigated back to {_state.CurrentUrl}");
        }
        catch (Exception ex)
        {
            return ScraperResult.Error($"Go back failed: {ex.Message}");
        }
    }

    public async Task<ScraperResult> GoForwardAsync()
    {
        try
        {
            EnsurePageExists();
            
            await _page!.GoForwardAsync();
            await UpdateStateAsync();
            
            return ScraperResult.Ok($"Navigated forward to {_state.CurrentUrl}");
        }
        catch (Exception ex)
        {
            return ScraperResult.Error($"Go forward failed: {ex.Message}");
        }
    }

    public async Task<ScraperResult> RefreshAsync()
    {
        try
        {
            EnsurePageExists();
            
            await _page!.ReloadAsync();
            await UpdateStateAsync();
            
            return ScraperResult.Ok($"Refreshed {_state.CurrentUrl}");
        }
        catch (Exception ex)
        {
            return ScraperResult.Error($"Refresh failed: {ex.Message}");
        }
    }

    public async Task<ScraperResult> ScrollAsync(string direction, int pixels = 300)
    {
        try
        {
            EnsurePageExists();
            
            var scrollAmount = direction.ToLower() == "up" ? -pixels : pixels;
            await _page!.EvaluateExpressionAsync($"window.scrollBy(0, {scrollAmount})");
            
            return ScraperResult.Ok($"Scrolled {direction} {pixels}px");
        }
        catch (Exception ex)
        {
            return ScraperResult.Error($"Scroll failed: {ex.Message}");
        }
    }

    public async Task<ScraperResult> EvaluateScriptAsync(string script)
    {
        try
        {
            EnsurePageExists();
            
            var result = await _page!.EvaluateExpressionAsync(script);
            var resultStr = result?.ToString() ?? "undefined";
            
            return ScraperResult.Ok(data: resultStr);
        }
        catch (Exception ex)
        {
            return ScraperResult.Error($"Script evaluation failed: {ex.Message}");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_page != null)
        {
            await _page.CloseAsync();
        }
        
        if (_browser != null)
        {
            await _browser.CloseAsync();
        }
        
        _state.IsConnected = false;
    }

    private void EnsurePageExists()
    {
        if (_page == null)
        {
            throw new InvalidOperationException("Browser not initialized. Call InitializeAsync first.");
        }
    }

    private async Task UpdateStateAsync()
    {
        if (_page != null)
        {
            _state.CurrentUrl = _page.Url;
            _state.PageTitle = await _page.GetTitleAsync();
            _state.LastNavigationTime = DateTime.Now;
        }
    }

    private static async Task<string> FormatHtmlAsync(string html)
    {
        try
        {
            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(html));
            
            using var writer = new StringWriter();
            document.ToHtml(writer, new PrettyMarkupFormatter());
            return writer.ToString();
        }
        catch
        {
            return html;
        }
    }
}
