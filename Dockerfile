# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution file
COPY WebScraper.sln ./

# Copy project files
COPY src/WebScraper.Domain/WebScraper.Domain.csproj ./src/WebScraper.Domain/
COPY src/WebScraper.Application/WebScraper.Application.csproj ./src/WebScraper.Application/
COPY src/WebScraper.Infrastructure/WebScraper.Infrastructure.csproj ./src/WebScraper.Infrastructure/
COPY src/WebScraper.CLI/WebScraper.CLI.csproj ./src/WebScraper.CLI/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY src/ ./src/

# Build and publish the application
WORKDIR /src/src/WebScraper.CLI
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app

# Install Chromium dependencies for PuppeteerSharp
RUN apt-get update && apt-get install -y \
    wget \
    ca-certificates \
    fonts-liberation \
    libasound2t64 \
    libatk-bridge2.0-0 \
    libatk1.0-0 \
    libatspi2.0-0 \
    libcairo2 \
    libcups2 \
    libdbus-1-3 \
    libdrm2 \
    libgbm1 \
    libglib2.0-0 \
    libgtk-3-0 \
    libnspr4 \
    libnss3 \
    libpango-1.0-0 \
    libx11-6 \
    libxcb1 \
    libxcomposite1 \
    libxdamage1 \
    libxext6 \
    libxfixes3 \
    libxkbcommon0 \
    libxrandr2 \
    xdg-utils \
    && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=build /app/publish .

# Set entrypoint
ENTRYPOINT ["dotnet", "WebScraper.CLI.dll"]

# Default command (can be overridden)
CMD ["--help"]