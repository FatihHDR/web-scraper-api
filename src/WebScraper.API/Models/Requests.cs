namespace WebScraper.API.Models;

public record NavigateRequest(string Url);

public record ClickRequest(string Selector);

public record TypeRequest(string Selector, string Text);

public record ScrollRequest(string Direction, int Pixels = 300);

public record ScriptRequest(string Script);
