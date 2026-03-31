# Yuki QA Automation Tests - Complete Setup Guide

## ? Project Successfully Created!

Your complete Playwright-based test automation framework for .NET 8.0 has been set up with best practices.

## ?? Project Structure

```
yuki-qa-automation-tests/
??? Configuration/
?   ??? BrowserConfig.cs          # Browser configuration settings
?   ??? TestSettings.cs           # Test settings from appsettings.json
??? Core/
?   ??? BaseTest.cs               # Base test class with setup/teardown
?   ??? DriverFactory.cs          # Browser instance factory
??? PageObjects/
?   ??? Pages/
?   ?   ??? BasePage.cs           # Base page object class
?   ?   ??? MenuPage.cs           # Menu navigation page object
?   ?   ??? InvoicesPage.cs       # Invoices page object
?   ??? Models/
?       ??? InvoiceRow.cs         # Invoice data model
??? Services/
?   ??? InvoiceService.cs         # Invoice business logic operations
?   ??? NavigationService.cs      # Navigation operations
??? Tests/
?   ??? Integration/
?       ??? InvoiceTests.cs       # Integration test cases
??? Utilities/
?   ??? Assertions.cs             # Custom assertion methods
?   ??? Logger.cs                 # Logging utility (file & console)
?   ??? WaitHelper.cs             # Explicit wait operations
??? appsettings.json              # Production configuration
??? appsettings.Development.json  # Development configuration
??? yuki-qa-automation-tests.csproj   # Project file (.NET 8.0)
```

## ?? Quick Start

### 1. Install Playwright Browsers
Run this command to install required browsers:
```powershell
cd "C:\Users\arunk\Downloads\qa-automation-test\yuki-qa-automation-test\yuki-qa-automation-tests\yuki-qa-automation-tests"
pwsh bin/Debug/net8.0/playwright.ps1 install
```

### 2. Start Your Application
Ensure the application is running at:
```
http://localhost:5000/
```

### 3. Run All Tests
```powershell
dotnet test
```

### 4. Run Specific Test
```powershell
dotnet test --filter "InvoiceTests.VerifyInvoiceAmount_I634_ShouldEqual_423_99_EUR"
```

### 5. Run with Detailed Output
```powershell
dotnet test --logger "console;verbosity=detailed"
```

## ?? Available Test Cases

### Test 1: Navigate to All Pages
**Test Name:** `NavigateToAllPages_UsingMenu_ShouldSucceed`

**What it does:**
- Starts at the home page
- Navigates to each menu item (Home, Invoices, Customers, Products)
- Verifies the menu is displayed on each page

**Run:**
```powershell
dotnet test --filter "NavigateToAllPages_UsingMenu_ShouldSucceed"
```

---

### Test 2: Verify Invoice Summary Total
**Test Name:** `VerifyInvoiceSummaryTotal_ShouldMatchSumOfAllInvoices`

**What it does:**
- Navigates to the Invoices page
- Retrieves all individual invoice amounts
- Calculates the sum
- Verifies the summary row total matches the calculated sum

**Run:**
```powershell
dotnet test --filter "VerifyInvoiceSummaryTotal_ShouldMatchSumOfAllInvoices"
```

---

### Test 3: Verify Specific Invoice Amount
**Test Name:** `VerifyInvoiceAmount_I634_ShouldEqual_423_99_EUR`

**What it does:**
- Navigates to the Invoices page
- Searches for invoice 'I634'
- Verifies the amount equals '423.99 EUR'

**Run:**
```powershell
dotnet test --filter "VerifyInvoiceAmount_I634_ShouldEqual_423_99_EUR"
```

## ?? Configuration

Edit `appsettings.json` to configure test behavior:

```json
{
  "TestSettings": {
    "BaseUrl": "http://localhost:5000/",
    "PageLoadTimeout": 30000,
    "ElementWaitTimeout": 10000,
    "BrowserConfig": {
      "BrowserType": "chromium",  // chromium, firefox, webkit
      "Headless": false,           // Set to true for headless mode
      "TimeoutMs": 30000,          // Element timeout in milliseconds
      "RecordVideo": false,        // Record browser videos
      "VideoPath": "./videos",     // Video storage location
      "CaptureScreenshot": true,   // Capture on test failure
      "ScreenshotPath": "./screenshots"
    }
  }
}
```

## ?? Architecture Overview

### Page Object Model (POM)
- **BasePage**: Contains common page operations (navigate, click, getText, etc.)
- **MenuPage**: Handles menu navigation (inherits from BasePage)
- **InvoicesPage**: Handles invoice-specific operations (inherits from MenuPage)

### Service Layer
- **NavigationService**: Orchestrates page navigation and verification
- **InvoiceService**: Handles invoice business logic and verification

### Core Framework
- **BaseTest**: Provides test lifecycle (setup, teardown), logging, configuration
- **DriverFactory**: Creates and manages Playwright browser instances

### Utilities
- **Logger**: Logs to both console and file (`logs/` folder)
- **WaitHelper**: Explicit waits for elements and page loads
- **Assertions**: Custom assertion methods for test validation

## ?? Output Folders (Auto-created)

- **logs/** - Daily test execution logs
- **screenshots/** - Test failure screenshots (if enabled)
- **videos/** - Browser recordings (if enabled)

## ?? Logging

All test execution is logged to:
```
logs/YYYY-MM-DD.log
```

Log format:
```
[2024-01-15 10:30:45.123] [INFO] [InvoiceTests] Test setup started
[2024-01-15 10:30:46.456] [INFO] [InvoiceTests] Creating chromium browser instance
```

## ?? Screenshots on Failure

Failed tests automatically capture screenshots to:
```
screenshots/TestName_YYYY-MM-DD_HH-mm-ss.png
```

Enable/disable in `appsettings.json`:
```json
"CaptureScreenshot": true
```

## ?? Troubleshooting

### Issue: "Element not found" errors
**Solution:**
- Verify CSS selectors in page objects match your HTML structure
- Increase `ElementWaitTimeout` in `appsettings.json`
- Check application is running at correct URL

### Issue: Browser doesn't launch
**Solution:**
- Install Playwright browsers: `pwsh bin/Debug/net8.0/playwright.ps1 install`
- Ensure .NET 8.0 is installed: `dotnet --version`

### Issue: Tests timeout
**Solution:**
- Increase `TimeoutMs` in `BrowserConfig`
- Check network/internet connectivity
- Ensure application is responsive

### Issue: "Summary row not found"
**Solution:**
- Verify the Invoices page has summary row in footer
- Check CSS selector `"table.table tfoot tr"` matches your HTML
- Update selector in `InvoicesPage.cs` if needed

## ?? Tips & Best Practices

1. **Headless Mode for CI/CD**: Set `"Headless": true` for automated pipelines
2. **Increase Video Recording**: Enable `"RecordVideo": true` for debugging test failures
3. **Change Browser**: Set `"BrowserType": "firefox"` or `"webkit"` to test cross-browser compatibility
4. **Parallel Execution**: Run tests with `dotnet test -- RunConfiguration.MaxCpuCount=4`
5. **Filter Tests**: Use `--filter` to run specific tests during development

## ?? Key Classes & Methods

### InvoicesPage
```csharp
await invoicesPage.NavigateToInvoicesPage()
await invoicesPage.GetAllInvoices()           // Returns List<InvoiceRow>
await invoicesPage.GetSummaryTotal()          // Returns decimal
await invoicesPage.GetInvoiceAmount("I634")   // Returns decimal
```

### NavigationService
```csharp
await navigationService.NavigateToAllPagesAsync()  // Returns bool
```

### InvoiceService
```csharp
await invoiceService.VerifySummaryTotalAsync()                    // Returns bool
await invoiceService.VerifyInvoiceAmountAsync("I634", 423.99m)   // Returns bool
await invoiceService.GetAllInvoicesAsync()                        // Returns List<InvoiceRow>
```

## ? Features

? **Async/Await Pattern** - Non-blocking operations throughout
? **Configuration-Driven** - Easy environment switching
? **Comprehensive Logging** - Console and file logging with timestamps
? **Page Object Model** - Clean separation of concerns
? **Service Layer** - Business logic separated from UI
? **Explicit Waits** - Reliable element waiting
? **Screenshot Capture** - Automatic on test failure
? **Base Test Infrastructure** - Automatic setup/teardown
? **Custom Assertions** - Clear, descriptive test messages
? **Cross-Browser Support** - Chromium, Firefox, WebKit

## ?? Next Steps

1. ? Verify your application is running at `http://localhost:5000/`
2. ? Install Playwright: `pwsh bin/Debug/net8.0/playwright.ps1 install`
3. ? Run tests: `dotnet test`
4. ? Check logs in `logs/` folder
5. ? View any failure screenshots in `screenshots/` folder

## ?? Support

For issues:
1. Check the logs in `logs/` folder for detailed execution traces
2. Check screenshots in `screenshots/` folder for visual debugging
3. Verify CSS selectors match your application HTML
4. Ensure Playwright browsers are installed: `pwsh bin/Debug/net8.0/playwright.ps1 install`

---

**Happy Testing! ??**
