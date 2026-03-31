# Yuki QA Automation Tests

A comprehensive, production-ready test automation framework built with Playwright and C#. Designed for robust execution in both local development and CI/CD pipeline environments.

## ?? Overview

This solution implements a complete automation testing framework following SOLID principles, design patterns, and industry best practices. It includes:

- **Playwright Integration**: Modern browser automation with support for Chromium, Firefox, and WebKit
- **CI/CD Optimizations**: Resilience patterns, retry logic, and performance tuning for unattended execution
- **Reusable Architecture**: Page Object Model (POM), Service layer, and base test classes
- **Comprehensive Logging**: Multi-level logging with both console and file output
- **Configuration Management**: Environment-specific settings with sensible defaults

## ?? Key Features

### Architecture & Design Patterns
- **Page Object Model (POM)**: Encapsulated page interactions in dedicated classes (`BasePage`, `InvoicesPage`, `MenuPage`)
- **Service Layer**: Business logic abstraction (`InvoiceService`, `NavigationService`)
- **Base Test Class**: Common setup/teardown logic with lifecycle management
- **Dependency Injection**: Services receive dependencies via constructor injection

### Automation Best Practices
- **Intelligent Wait Strategies**: Fallback mechanisms for robust element detection
- **Retry Logic**: Automatic retry on transient failures
- **Screenshot Capture**: Automatic screenshots on test failures
- **Structured Logging**: Detailed execution traces for diagnostics
- **Error Handling**: Comprehensive exception handling throughout

### Performance & CI/CD Optimization
- **Headless Mode**: Configured for CI/CD by default (can be overridden)
- **Resource Optimization**: 
  - Image/font blocking to reduce network traffic
  - Configurable timeouts for different environments
  - Network latency simulation support
- **Browser Arguments**: Linux-friendly arguments for containerized environments (`--disable-dev-shm-usage`, `--disable-gpu`)
- **Parallel Execution**: Support for test parallelization via NUnit
- **Environment Detection**: Automatic configuration switching (CI/CD vs. local development)

## ?? Getting Started

### Prerequisites
- .NET Core 3.1 or later
- Playwright browsers (automatically installed on first run)

### Installation

1. Clone the repository
```bash
git clone <repository-url>
cd yuki-qa-automation-tests
```

2. Restore dependencies
```bash
dotnet restore
```

3. Install Playwright browsers (if not auto-installed)
```bash
pwsh bin/Debug/netcoreapp3.1/playwright.ps1 install
```

### Local Development Setup

1. Ensure the test application is running on `http://localhost:5000/`

2. Run tests in UI mode (visible browser):
```bash
dotnet test --configuration Debug
```

3. Run specific test:
```bash
dotnet test --filter="VerifyInvoiceSummaryTotal"
```

## ?? Configuration

### Environment-Specific Settings

Configuration is managed through `appsettings.json` and `appsettings.{ENVIRONMENT}.json`:

- **appsettings.json**: CI/CD defaults (headless mode, optimized timeouts, resource blocking)
- **appsettings.Development.json**: Local development (UI mode, longer timeouts, lower retry attempts)

### Key Configuration Options

```json
{
  "TestSettings": {
    "BaseUrl": "http://localhost:5000/",
    "PageLoadTimeout": 30000,              // ms
    "ElementWaitTimeout": 10000,           // ms
    "NavigationTimeout": 30000,            // ms
    "RetryAttempts": 3,                    // For CI/CD resilience
    "RetryDelayMs": 1000,
    "BrowserConfig": {
      "BrowserType": "chromium",
      "Headless": true,                    // false for local dev
      "TimeoutMs": 30000,
      "NavigationTimeoutMs": 30000,
      "RecordVideo": false,
      "CaptureScreenshot": true,
      "ScreenshotPath": "./screenshots",
      "Args": [],                          // Browser launch arguments
      "EnvironmentVariables": {}
    }
  }
}
```

### Setting Environment Variables

#### Local Development
```bash
# Use Development settings
set ASPNETCORE_ENVIRONMENT=Development

# Or use default (Development)
dotnet test
```

#### CI/CD Pipeline (Azure Pipelines, GitHub Actions, etc.)
```bash
# Use Production settings (appsettings.json defaults)
set ASPNETCORE_ENVIRONMENT=Production

# Or CI/CD auto-detection
set CI=true
dotnet test --logger:"console;verbosity=detailed"
```

## ?? Project Structure

```
yuki-qa-automation-tests/
??? Configuration/           # Configuration classes
?   ??? BrowserConfig.cs    # Browser-specific settings
?   ??? TestSettings.cs     # Overall test settings
??? Core/                   # Core infrastructure
?   ??? BaseTest.cs         # Base test class with lifecycle
?   ??? DriverFactory.cs    # Playwright browser factory
??? PageObjects/            # Page Object Model
?   ??? Pages/
?   ?   ??? BasePage.cs     # Base page with common actions
?   ?   ??? InvoicesPage.cs # Invoice page interactions
?   ?   ??? MenuPage.cs     # Menu navigation
?   ??? Models/
?       ??? InvoiceRow.cs   # Data models
??? Services/               # Business logic layer
?   ??? InvoiceService.cs   # Invoice operations
?   ??? NavigationService.cs# Navigation operations
??? Tests/Integration/      # Test cases
?   ??? InvoiceTests.cs     # Invoice-related tests
??? Utilities/              # Helper utilities
?   ??? Logger.cs           # Logging utility
?   ??? WaitHelper.cs       # Wait strategies
?   ??? Assertions.cs       # Custom assertions
??? appsettings.json        # CI/CD defaults
??? appsettings.Development.json  # Dev environment
??? README.md              # This file
```

## ?? Test Execution

### Run All Tests
```bash
dotnet test
```

### Run Tests with Specific Filter
```bash
dotnet test --filter="VerifyInvoiceAmount"
```

### Run Tests with Detailed Logging
```bash
dotnet test --logger:"console;verbosity=detailed"
```

### Run Tests in Parallel
```bash
dotnet test /p:ParallelizeAssembly=true /p:ParallelizeTestCollections=true
```

### CI/CD Execution Example (Azure Pipelines)
```yaml
- task: DotNetCoreCLI@2
  inputs:
    command: 'test'
    arguments: '--configuration Release --logger "console;verbosity=detailed" --results-directory $(Build.ArtifactStagingDirectory)'
  env:
    ASPNETCORE_ENVIRONMENT: Production
```

## ?? Output & Artifacts

### Test Results
- NUnit XML reports: `TestResults/`
- Screenshots: `./screenshots/` (on failure)
- Logs: `./logs/` (daily rolling files)
- Videos: `./videos/` (if enabled in config)

### Accessing Logs
```bash
# View today's log
tail -f logs/2024-01-15.log

# View specific test log
grep "InvoiceTests" logs/2024-01-15.log
```

## ?? Implemented Test Cases

### 1. Navigation Test
- **Test**: `NavigateToAllPages_UsingMenu_ShouldSucceed`
- **Purpose**: Verify menu navigation works across all pages
- **Coverage**: Menu interaction, page navigation

### 2. Invoice Summary Verification
- **Test**: `VerifyInvoiceSummaryTotal_ShouldMatchSumOfAllInvoices`
- **Purpose**: Validate invoice totals calculation
- **Coverage**: Data aggregation, calculation accuracy

### 3. Specific Invoice Amount Verification
- **Test**: `VerifyInvoiceAmount_I634_ShouldEqual_423_99_EUR`
- **Purpose**: Verify specific invoice amounts are correct
- **Coverage**: Data retrieval, value validation

## ??? Resilience & Performance Features

### Automatic Retry Logic
- Browser initialization: 3 retries with 1-second delays
- Element detection: Retry with fallback wait strategies
- Navigation: Dual-mode wait (NetworkIdle ? DOMContentLoaded)

### Performance Optimizations
- **Resource Blocking**: Images and fonts blocked by default (configurable)
- **Timeout Tuning**: Separate timeouts for navigation, page load, and element wait
- **Network Simulation**: Support for slow network simulation via `SlowMo` config
- **Headless Rendering**: Fast, minimal-overhead browser automation

### CI/CD-Specific Optimizations
- Linux-compatible browser launch arguments
- Automatic detection of CI/CD environments (Azure Pipelines, GitHub Actions, etc.)
- Memory-safe operations (`--disable-dev-shm-usage`)
- GPU-disabled for container compatibility (`--disable-gpu`)

## ?? Troubleshooting

### Tests Fail in CI/CD but Pass Locally
1. Check `appsettings.json` timeouts - increase if needed
2. Enable debug logging: `set CI=true` (enables debug output)
3. Review video/screenshots in artifacts directory
4. Verify test application is accessible at configured URL

### Browser Launch Failures
```bash
# Manually install browsers
pwsh bin/Debug/netcoreapp3.1/playwright.ps1 install

# Or for all browsers
pwsh bin/Debug/netcoreapp3.1/playwright.ps1 install-deps
```

### Timeout Issues
Increase timeouts in `appsettings.json`:
```json
{
  "TestSettings": {
    "PageLoadTimeout": 60000,    // Increased from 30000
    "NavigationTimeout": 60000   // Increased from 30000
  }
}
```

### Element Not Found
1. Enable debug logging for selector details
2. Check page screenshots in `./screenshots/`
3. Verify selector is correct in browser DevTools
4. Increase `ElementWaitTimeout` if site is slow

## ?? Best Practices Implemented

### Code Quality
- ? SOLID principles (Single Responsibility, Dependency Injection)
- ? Comprehensive XML documentation
- ? Null-safe operations (null-coalescing operators)
- ? Async/await for non-blocking operations
- ? Consistent error handling

### Testing Quality
- ? Descriptive test names
- ? Clear arrange-act-assert pattern
- ? Independent test cases
- ? Deterministic test execution
- ? Comprehensive logging

### DevOps & CI/CD
- ? Environment-based configuration
- ? Headless-first approach
- ? Automatic artifact collection
- ? Performance-optimized for pipelines
- ? Container-friendly setup

## ?? Contributing

Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines on how to contribute to this project.

## ?? License

This project is licensed under the MIT License.

## ?? Support

For issues, questions, or contributions, please open an issue on the repository.

---

**Last Updated**: January 2024  
**Framework Version**: Playwright 1.40.0  
**.NET Version**: Core 3.1+
