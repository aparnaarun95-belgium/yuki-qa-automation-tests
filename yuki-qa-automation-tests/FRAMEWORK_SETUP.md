# Yuki QA Automation Tests - Setup Guide

## Overview

This is a production-ready test automation framework built with:
- **Language**: C# (.NET 8)
- **Test Framework**: NUnit
- **Browser Automation**: Playwright
- **Architecture**: Page Object Model (POM) with base classes

## Project Structure

```
yuki-qa-automation-tests/
??? Base/
?   ??? BaseTest.cs                 # Base class for all tests
??? Configuration/
?   ??? PlaywrightConfiguration.cs  # Configuration management
??? Drivers/
?   ??? BrowserManager.cs           # Browser lifecycle management
?   ??? ContextManager.cs           # Context and page management
??? Pages/
?   ??? BasePage.cs                 # Base page object model
?   ??? HomePage.cs                 # Home page page object
??? Tests/
?   ??? HomePageTests.cs            # Home page test cases
?   ??? SignInTests.cs              # Sign in test cases
?   ??? DataDrivenTests.cs          # Data-driven test examples
??? Utilities/
?   ??? WaitHelper.cs               # Wait and polling utilities
?   ??? ScreenshotHelper.cs         # Screenshot capture
?   ??? RetryHelper.cs              # Retry logic for resilience
?   ??? PerformanceHelper.cs        # Performance metrics
??? Config/
?   ??? appsettings.json            # Production configuration
?   ??? appsettings.Development.json # Development configuration
??? yuki-qa-automation-tests.csproj # Project file
```

## Getting Started

### Prerequisites

- .NET 8 SDK installed
- Visual Studio 2022 or VS Code
- Internet connection (for browser downloads)

### Installation

1. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

2. **Install Playwright browsers** (first time only):
   ```bash
   pwsh bin/Release/net8.0/playwright.ps1 install
   ```

   Or using .NET:
   ```bash
   dotnet build
   ```

### Configuration

Edit `Config/appsettings.json` to set:
- `BaseUrl`: Your target application URL
- `BrowserType`: chromium, firefox, or webkit
- `Headless`: true for CI/CD, false for debugging
- `Timeout`: Maximum wait time in milliseconds

## Running Tests

### Run all tests
```bash
dotnet test
```

### Run specific test class
```bash
dotnet test --filter "TestClass=yuki_qa_automation_tests.Tests.HomePageTests"
```

### Run tests by category
```bash
dotnet test --filter "Category=Smoke"
```

### Run with verbose output
```bash
dotnet test -v normal
```

### Run with code coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutput=coverage/
```

## Best Practices Implemented

### 1. **Page Object Model (POM)**
- Separates page logic from test logic
- Centralized element selectors
- Reusable page interactions
- Easy maintenance and updates

### 2. **Base Classes**
- Common setup/teardown logic in `BaseTest`
- Shared element operations in `BasePage`
- Consistent logging and failure handling

### 3. **Configuration Management**
- Environment-specific settings
- External configuration files
- No hardcoded values

### 4. **Wait Strategies**
- Explicit waits instead of implicit
- Load state checks for network idle
- Retry logic for flaky operations
- Timeout management

### 5. **Error Handling**
- Automatic screenshot on failure
- Detailed test logging
- Proper resource cleanup
- Exception aggregation for retries

### 6. **Performance Optimization**
- Parallel test execution support
- Network idle waits (not fixed delays)
- Browser context reuse
- Efficient element selectors
- Performance metrics tracking

### 7. **CI/CD Pipeline Support**
- Headless browser mode by default
- No UI interactions required
- Configurable timeouts for various environments
- Screenshot artifacts for debugging
- Structured logging output

## Creating New Tests

### 1. Create a Page Object (if needed)

```csharp
public class LoginPage : BasePage
{
    private const string UsernameInput = "[data-testid='username']";
    private const string PasswordInput = "[data-testid='password']";
    private const string LoginButton = "button:has-text('Login')";

    public LoginPage(IPage page, int defaultTimeout = 10000)
        : base(page, defaultTimeout)
    {
    }

    public async Task LoginAsync(string username, string password)
    {
        await FillAsync(UsernameInput, username);
        await FillAsync(PasswordInput, password);
        await ClickAsync(LoginButton);
    }
}
```

### 2. Create Test Class

```csharp
[TestFixture]
[Category("UI")]
public class LoginTests : BaseTest
{
    private LoginPage _loginPage;

    [SetUp]
    public async Task TestSetup()
    {
        _loginPage = new LoginPage(Page);
        BaseUrl = "https://example.com/login";
    }

    [Test]
    public async Task VerifySuccessfulLogin()
    {
        // Arrange
        await NavigateToAsync(BaseUrl);

        // Act
        await _loginPage.LoginAsync("user@example.com", "password123");

        // Assert
        Assert.That(Page.Url, Contains.Substring("/dashboard"));
    }
}
```

## Test Naming Convention

Tests follow this format:
```
TC[Number]_[Descriptive Name]
```

Example: `TC001_VerifyHomePageLoads`

## Logging and Debugging

Tests write output to the NUnit output:
```csharp
TestContext.WriteLine("? Test passed");
TestContext.WriteLine($"Current URL: {Page.Url}");
```

## Screenshots

Failed tests automatically capture screenshots in the `Screenshots/` directory with naming:
```
TestName_yyyyMMdd_HHmmss_FAILURE.png
```

## Troubleshooting

### Tests timeout in CI/CD
- Increase `NavigationTimeout` in `appsettings.json`
- Check network latency in CI/CD environment
- Verify base URL is accessible

### Elements not found
- Check selector accuracy with browser DevTools
- Use explicit waits (`WaitForElementAsync`)
- Review page loading state

### Flaky tests
- Add appropriate waits for network requests
- Use `WaitForLoadStateAsync(LoadState.NetworkIdle)`
- Implement retry logic with `RetryHelper`

## Performance Metrics

Use `PerformanceHelper` to track timing:
```csharp
var perf = new PerformanceHelper();
perf.Start();
await _page.GotoAsync(url);
perf.Stop();
TestContext.WriteLine($"Page load: {perf.GetFormattedElapsedTime()}");
```

## Next Steps

1. Update `Config/appsettings.json` with your target URL
2. Implement additional page objects for your application
3. Add more test cases based on your test plan
4. Configure CI/CD pipeline to run tests
5. Monitor test execution and adjust timeouts as needed

## Support

For Playwright documentation: https://playwright.dev/dotnet/
For NUnit documentation: https://docs.nunit.org/
