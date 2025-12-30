# Web Scraper CLI

A professional CLI web scraper tool built with .NET 10, Clean Architecture, and PuppeteerSharp for headless browser automation.

## Features

- **Headless Browser Navigation** - Navigate websites using Chrome/Chromium
- **HTML Capture** - Extract page content using CSS selectors
- **Interactive Elements** - Click buttons, fill forms, and interact with pages
- **Screenshot Capture** - Save visual snapshots of pages
- **JavaScript Execution** - Run custom scripts in the browser context
- **Syntax-Highlighted Output** - Beautiful console output with colors

## Available Commands

| Command | Description | Example |
|---------|-------------|---------|
| `navigate <url>` | Navigate to a URL | `navigate https://example.com` |
| `show code` | Display page HTML | `show code` |
| `capture <selector>` | Get element(s) by CSS selector | `capture h1` |
| `click on <selector>` | Click on an element | `click on button.submit` |
| `type <selector> <text>` | Type text into an input | `type #search hello world` |
| `screenshot [path]` | Take a screenshot | `screenshot page.png` |
| `wait <ms>` | Wait for milliseconds | `wait 2000` |
| `back` | Go back in history | `back` |
| `forward` | Go forward in history | `forward` |
| `refresh` | Reload the page | `refresh` |
| `scroll <up\|down> [px]` | Scroll the page | `scroll down 500` |
| `eval <script>` | Execute JavaScript | `eval document.title` |
| `clear` | Clear the console | `clear` |
| `help` | Show available commands | `help` |
| `exit` | Close browser and exit | `exit` |

## Requirements

- .NET 10 SDK
- Windows/Linux/macOS

## Installation

```bash
# Clone the repository
git clone <repository-url>
cd web-scraper

# Build the project
dotnet build

# Run the CLI
dotnet run --project src/WebScraper.CLI
```

## Project Structure

```
web-scraper/
├── src/
│   ├── WebScraper.Domain/           # Core entities & interfaces
│   │   ├── Entities/
│   │   │   ├── BrowserState.cs
│   │   │   └── ScraperResult.cs
│   │   └── Interfaces/
│   │       └── IBrowserService.cs
│   │
│   ├── WebScraper.Application/      # Commands & services
│   │   ├── Commands/
│   │   │   ├── ICommand.cs
│   │   │   ├── NavigateCommand.cs
│   │   │   ├── ShowCodeCommand.cs
│   │   │   ├── CaptureCommand.cs
│   │   │   └── ... (other commands)
│   │   └── Services/
│   │       └── CommandParser.cs
│   │
│   ├── WebScraper.Infrastructure/   # PuppeteerSharp implementation
│   │   ├── Services/
│   │   │   └── PuppeteerBrowserService.cs
│   │   └── DependencyInjection.cs
│   │
│   └── WebScraper.CLI/              # Console application
│       ├── Helpers/
│       │   └── ConsoleHelper.cs
│       └── Program.cs
│
├── WebScraper.sln
└── README.md
```

## Usage Examples

### Basic Navigation
```
scraper> navigate https://news.ycombinator.com
✓ Navigated to https://news.ycombinator.com (Title: Hacker News)

scraper> capture .titleline
✓ Found 30 element(s)
<span class="titleline">
  <a href="...">Article Title</a>
</span>
...
```

### Form Interaction
```
scraper> navigate https://google.com
✓ Navigated to https://www.google.com (Title: Google)

scraper> type textarea[name="q"] web scraping tutorial
✓ Typed "web scraping tutorial" into textarea[name="q"]

scraper> click on input[name="btnK"]
✓ Clicked on element: input[name="btnK"]
```

### Screenshot Capture
```
scraper> navigate https://github.com
✓ Navigated to https://github.com (Title: GitHub)

scraper> screenshot github-homepage.png
✓ Screenshot saved to D:\web-scraper\github-homepage.png
```

### JavaScript Execution
```
scraper> navigate https://example.com
✓ Navigated to https://example.com (Title: Example Domain)

scraper> eval document.querySelectorAll('a').length
2

scraper> eval document.title
"Example Domain"
```

## Architecture

This project follows **Clean Architecture** principles:

- **Domain Layer**: Core business logic and interfaces (no dependencies)
- **Application Layer**: Use cases, commands, and service orchestration
- **Infrastructure Layer**: External service implementations (PuppeteerSharp)
- **Presentation Layer**: CLI user interface

## License

MIT License
