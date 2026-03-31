# Playwright Test Automation Framework - Complete Setup Guide

## ? What Has Been Set Up

A **production-ready test automation framework** with the following components:

### Framework Components

1. **Base Classes**
   - `BaseTest.cs` - Base test class with setup/teardown and browser lifecycle
   - `BasePage.cs` - Base page object with common element interactions

2. **Drivers & Management**
   - `BrowserManager.cs` - Browser launch and lifecycle management
   - `ContextManager.cs` - Context and page creation/management

3. **Page Objects**
   - `HomePage.cs` - Example page object for home page

4. **Test Cases**
   - `HomePageTests.cs` - 10 comprehensive home page tests (TC001-TC010)
   - `SignInTests.cs` - Sign-in related tests (TC011-TC013)
   - `DataDrivenTests.cs` - Data-driven test examples (TC014)

5. **Utilities**
   - `WaitHelper.cs` - Smart wait strategies and element polling
   - `ScreenshotHelper.cs` - Automatic screenshot capture
   - `RetryHelper.cs` - Retry logic for resilient tests
   - `PerformanceHelper.cs` - Performance metrics tracking

6. **Configuration**
   - `PlaywrightConfiguration.cs` - Configuration management
   - `appsettings.json` - Production settings
   - `appsettings.Development.json` - Development settings

---

## ?? Quick Start

### Step 1: Restore NuGet Packages
```bash
dotnet restore
```

### Step 2: Install Playwright Browsers

**For first-time setup**, run this PowerShell command:
```bash
pwsh -Command "& {Invoke-WebRequest -Uri https://aka.ms/playwright-cli -OutFile pwsh.zip; Expand-Archive pwsh.zip; .\pwsh\playwright.exe install}"
```

**Or simpler - using .NET:**
```bash
dotnet tool install --global Microsoft.Playwright.CLI
playwright install
```

### Step 3: Configure Target URL

Edit `Config/appsettings.json`:
```json
{
  "PlaywrightSettings": {
    "BaseUrl": "https://your-application-url.com",
    ...
  }
}
```

### Step 4: Run Tests
```bash
dotnet test
```

---

## ?? Test Results & Outputs

### Running Tests with Different Options

**Run all tests:**
```bash
dotnet test
```

**Run specific test class:**
```bash
dotnet test --filter "ClassName=yuki_qa_automation_tests.Tests.HomePageTests"
```

**Run tests by category (Smoke tests):**
```bash
dotnet test --filter "Category=Smoke"
```

**Run with verbose logging:**
```bash
dotnet test -v normal
```

**Run specific test:**
```bash
dotnet test --filter "Name=TC001_VerifyHomePageLoads"
```

---

## ?? Test Cases Overview

### Home Page Tests (TC001-TC010)
| ID | Description | Category |
|----|-------------|----------|
| TC001 | Verify home page loads successfully | Smoke |
| TC002 | Verify welcome heading is visible | Smoke |
| TC003 | Verify sign in button visible | Smoke |
| TC004 | Verify sign up button visible | Smoke |
| TC005 | Verify navigation menu present | Smoke |
| TC006 | Verify search functionality present | Smoke |
| TC007 | Verify featured section visible | Smoke |
| TC008 | Verify page title correct | Smoke |
| TC009 | Verify page responsiveness | Smoke |
| TC010 | Verify page load time < 10s | Smoke |

### Sign-In Tests (TC011-TC013)
| ID | Description | Category |
|----|-------------|----------|
| TC011 | Verify sign in button clickable | SignIn |
| TC012 | Verify sign up button clickable | SignIn |
| TC013 | Verify search functionality works | SignIn |

### Data-Driven Tests (TC014+)
- TC014: Search with multiple terms (parameterized test)

---

## ??? Project Architecture

```
yuki-qa-automation-tests/
?
??? Base/
?   ??? BaseTest.cs                 # [SetUp] [TearDown] browser lifecycle
?
??? Configuration/
?   ??? PlaywrightConfiguration.cs  # Config file reader
?
??? Drivers/
?   ??? BrowserManager.cs           # Browser launch/cleanup
?   ??? ContextManager.cs           # Context/page management
?
??? Pages/
?   ??? BasePage.cs                 # Common page methods
?   ??? HomePage.cs                 # HomePage POM
?
??? Tests/
?   ??? HomePageTests.cs            # 10 test cases
?   ??? SignInTests.cs              # 3 test cases
?   ??? DataDrivenTests.cs          # Parameterized tests
?
??? Utilities/
?   ??? WaitHelper.cs               # Smart waits
?   ??? ScreenshotHelper.cs         # Screenshots
?   ??? RetryHelper.cs              # Retry logic
?   ??? PerformanceHelper.cs        # Performance metrics
?
??? Config/
?   ??? appsettings.json            # Production config
?   ??? appsettings.Development.json # Dev config
?
??? yuki-qa-automation-tests.csproj # Project file

```

---

## ??? Best Practices Implemented

### 1. Page Object Model (POM)
```csharp
public class HomePage : BasePage
{
    private const string SignInButton = "button:has-text('Sign In')";

    public async Task ClickSignInAsync()
    {
        await ClickAsync(SignInButton);
    }
}
```

### 2. Base Test Class Pattern
```csharp
[TestFixture]
public class MyTests : BaseTest
{
    [SetUp]
    public async Task Setup() { }

    [Test]
    public async Task MyTest()
    {
        await NavigateToAsync(BaseUrl);
        // Test logic
    }
}
```

### 3. Explicit Waits (No Thread.Sleep)
```csharp
await element.WaitForAsync(new LocatorWaitForOptions 
{ 
    State = WaitForSelectorState.Visible 
});
```

### 4. Async/Await for Non-Blocking I/O
```csharp
await Page.GotoAsync(url, new PageGotoOptions 
{ 
    WaitUntil = WaitUntilState.NetworkIdle 
});
```

### 5. Automatic Failure Screenshot
```csharp
// Automatically captured in [TearDown]
// if TestStatus != Passed
// Saved as: Screenshots/TestName_yyyyMMdd_HHmmss_FAILURE.png
```

### 6. Configurable for CI/CD
```json
{
  "Headless": true,        // CI/CD
  "SlowMo": 0,             // CI/CD
  "ScreenshotOnFailure": true
}
```

---

## ?? Customization Guide

### Adding a New Page Object

1. Create `Pages/LoginPage.cs`:
```csharp
public class LoginPage : BasePage
{
    private const string UsernameInput = "[data-testid='username']";
    private const string PasswordInput = "[data-testid='password']";
    private const string LoginButton = "button[type='submit']";

    public LoginPage(IPage page) : base(page) { }

    public async Task LoginAsync(string username, string password)
    {
        await FillAsync(UsernameInput, username);
        await FillAsync(PasswordInput, password);
        await ClickAsync(LoginButton);
    }

    public async Task<bool> IsLoggedInAsync()
    {
        return await IsElementVisibleAsync("[data-testid='dashboard']");
    }
}
```

### Adding a New Test Class

2. Create `Tests/LoginTests.cs`:
```csharp
[TestFixture]
[Category("Authentication")]
public class LoginTests : BaseTest
{
    private LoginPage _loginPage;

    [SetUp]
    public async Task Setup()
    {
        _loginPage = new LoginPage(Page);
        BaseUrl = "https://example.com/login";
    }

    [Test]
    public async Task TC015_VerifySuccessfulLogin()
    {
        // Arrange
        await NavigateToAsync(BaseUrl);

        // Act
        await _loginPage.LoginAsync("user@example.com", "password123");

        // Assert
        Assert.That(await _loginPage.IsLoggedInAsync(), Is.True);
    }
}
```

---

## ?? Configuration Options

### appsettings.json (Production)
```json
{
  "PlaywrightSettings": {
    "BaseUrl": "https://example.com",      // Target URL
    "BrowserType": "chromium",              // chromium, firefox
    "Headless": true,                       // true for CI/CD
    "SlowMo": 0,                            // Milliseconds to slow down
    "Timeout": 30000,                       // Element wait timeout (ms)
    "NavigationTimeout": 30000,             // Page nav timeout (ms)
    "ScreenshotOnFailure": true,            // Capture on failure
    "VideoOnFailure": false,                // Record video on failure
    "TraceOnFailure": false                 // Record trace on failure
  },
  "RetryPolicy": {
    "MaxRetries": 3,                        // Number of retries
    "DelayMilliseconds": 1000               // Delay between retries
  },
  "Waits": {
    "DefaultWaitTimeMs": 5000,              // Default element wait
    "ElementWaitTimeMs": 10000,             // Specific element wait
    "NavigationWaitTimeMs": 30000           // Navigation wait
  }
}
```

### appsettings.Development.json (Local Testing)
```json
{
  "PlaywrightSettings": {
    "BaseUrl": "https://localhost:3000",
    "Headless": false,                      // Show browser
    "SlowMo": 500,                          // Slow down for visibility
    "VideoOnFailure": true,                 // Record video locally
    "TraceOnFailure": true                  // Record trace locally
  }
}
```

---

## ?? Troubleshooting

### Issue: "Browser not installed"
**Solution:**
```bash
playwright install chromium
```

### Issue: "Connection timeout in CI/CD"
**Solution:** Edit `appsettings.json`
```json
"NavigationTimeout": 60000,  // Increase to 60s
"Timeout": 60000
```

### Issue: "Element not found"
**Solution:**
1. Verify selector in browser DevTools
2. Add explicit wait:
```csharp
await WaitForElementVisibleAsync(selector);
```
3. Check if element is inside iframe (needs Page.FrameLocator)

### Issue: "Flaky tests"
**Solution:**
1. Use `LoadState.NetworkIdle` instead of `DOMContentLoaded`
2. Add retry logic:
```csharp
var helper = new RetryHelper(maxRetries: 3);
await helper.RetryAsync(async () => { /* test */ });
```

---

## ?? Performance Tips

### 1. Use Network Idle for Complete Loading
```csharp
await Page.GotoAsync(url, new PageGotoOptions 
{ 
    WaitUntil = WaitUntilState.NetworkIdle 
});
```

### 2. Run Tests in Parallel
```bash
dotnet test -- NUnit.NumberOfTestWorkers=4
```

### 3. Use Performance Helper for Metrics
```csharp
var perf = new PerformanceHelper();
perf.Start();
await Page.GotoAsync(url);
perf.Stop();
Assert.That(perf.GetElapsedSeconds(), Is.LessThan(5));
```

### 4. Disable Unnecessary Features
```json
"VideoOnFailure": false,    // Only for debugging
"TraceOnFailure": false,    // Only for debugging
"SlowMo": 0                 // CI/CD should be 0
```

---

## ?? CI/CD Pipeline Integration

### GitHub Actions Example
```yaml
name: E2E Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - uses: actions/setup-dotnet@v1
        with:
          dotnet-version: '8.0'
      - run: dotnet restore
      - run: dotnet build
      - run: playwright install
      - run: dotnet test --logger "console;verbosity=detailed"
      - name: Upload Screenshots
        if: failure()
        uses: actions/upload-artifact@v2
        with:
          name: screenshots
          path: Screenshots/
```

---

## ?? Files Reference

| File | Purpose | Key Class |
|------|---------|-----------|
| BaseTest.cs | Test base class | BaseTest |
| BasePage.cs | Page base class | BasePage |
| HomePage.cs | Home page POM | HomePage |
| BrowserManager.cs | Browser lifecycle | BrowserManager |
| ContextManager.cs | Context mgmt | ContextManager |
| WaitHelper.cs | Wait strategies | WaitHelper |
| RetryHelper.cs | Retry logic | RetryHelper |
| PerformanceHelper.cs | Perf metrics | PerformanceHelper |
| ScreenshotHelper.cs | Screenshots | ScreenshotHelper |

---

## ? Key Features

? **Async/Await throughout** - Non-blocking operations
? **Smart Wait Strategies** - Explicit waits, no Thread.Sleep
? **Page Object Model** - Maintainable test structure
? **Automatic Screenshots** - On test failure
? **Retry Logic** - Handle flaky tests
? **Performance Tracking** - Monitor test execution
? **Environment Config** - Dev vs Production settings
? **CI/CD Ready** - Headless, configurable timeouts
? **Comprehensive Logging** - Detailed test output
? **Data-Driven Tests** - Parameterized test support

---

## ?? Learning Resources

- [Playwright .NET Documentation](https://playwright.dev/dotnet/)
- [NUnit Documentation](https://docs.nunit.org/)
- [Page Object Model Pattern](https://www.selenium.dev/documentation/test_practices/encouraged/page_object_models/)
- [C# Async/Await](https://docs.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/)

---

## ?? Next Steps

1. ? Build the solution: `dotnet build`
2. ? Install browsers: `playwright install`
3. ? Update Config/appsettings.json with your URL
4. ? Run tests: `dotnet test`
5. ? Create more page objects and tests
6. ? Integrate with CI/CD

**Happy Testing! ??**
