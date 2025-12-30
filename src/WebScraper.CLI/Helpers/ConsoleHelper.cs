namespace WebScraper.CLI.Helpers;

/// <summary>
/// Helper class for styled console output.
/// </summary>
public static class ConsoleHelper
{
    public static void WriteHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════════╗
║                                                               ║
║   ██╗    ██╗███████╗██████╗     ███████╗ ██████╗██████╗      ║
║   ██║    ██║██╔════╝██╔══██╗    ██╔════╝██╔════╝██╔══██╗     ║
║   ██║ █╗ ██║█████╗  ██████╔╝    ███████╗██║     ██████╔╝     ║
║   ██║███╗██║██╔══╝  ██╔══██╗    ╚════██║██║     ██╔══██╗     ║
║   ╚███╔███╔╝███████╗██████╔╝    ███████║╚██████╗██║  ██║     ║
║    ╚══╝╚══╝ ╚══════╝╚═════╝     ╚══════╝ ╚═════╝╚═╝  ╚═╝     ║
║                                                               ║
║            🕷️  Web Scraper CLI - Headless Browser  🕷️         ║
║                                                               ║
╚═══════════════════════════════════════════════════════════════╝
");
        Console.ResetColor();
        Console.WriteLine("Type 'help' to see available commands, 'exit' to quit.\n");
    }

    public static void WritePrompt(string? url = null)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        if (!string.IsNullOrEmpty(url))
        {
            var shortUrl = url.Length > 50 ? url[..47] + "..." : url;
            Console.Write($"[{shortUrl}] ");
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("scraper> ");
        Console.ResetColor();
    }

    public static void WriteSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✓ {message}");
        Console.ResetColor();
    }

    public static void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✗ {message}");
        Console.ResetColor();
    }

    public static void WriteInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"ℹ {message}");
        Console.ResetColor();
    }

    public static void WriteWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"⚠ {message}");
        Console.ResetColor();
    }

    public static void WriteData(string? data)
    {
        if (string.IsNullOrEmpty(data)) return;

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(data);
        Console.ResetColor();
    }

    public static void WriteHtml(string html)
    {
        var lines = html.Split('\n');
        foreach (var line in lines)
        {
            WriteHtmlLine(line);
        }
    }

    private static void WriteHtmlLine(string line)
    {
        var inTag = false;

        foreach (var c in line)
        {
            if (c == '<')
            {
                inTag = true;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(c);
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if (c == '>')
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(c);
                Console.ResetColor();
                inTag = false;
            }
            else if (inTag && c == ' ')
            {
                Console.Write(c);
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (inTag && c == '=')
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(c);
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (inTag && (c == '"' || c == '\''))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(c);
            }
            else
            {
                Console.Write(c);
            }
        }
        Console.WriteLine();
        Console.ResetColor();
    }
}
