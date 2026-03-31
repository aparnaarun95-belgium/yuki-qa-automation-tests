# Quick Commands Reference

## ?? Getting Started

### Install Playwright Browsers (First time only)
```powershell
pwsh bin/Debug/net8.0/playwright.ps1 install
```

### Build the Project
```powershell
dotnet build
```

### Clean Build
```powershell
dotnet clean
dotnet build
```

## ?? Running Tests

### Run All Tests
```powershell
dotnet test
```

### Run All Tests with Verbose Output
```powershell
dotnet test --logger "console;verbosity=detailed"
```

### Run Specific Test by Name
```powershell
dotnet test --filter "NavigateToAllPages_UsingMenu_ShouldSucceed"
```

### Run All Invoice Tests
```powershell
dotnet test --filter "InvoiceTests"
```

### Run Tests in Headless Mode
Update `appsettings.json`:
```json
"Headless": false  ?  "Headless": true
```

### Run Tests with No Build
```powershell
dotnet test --no-build
```

### Run Tests with Specific Framework
```powershell
dotnet test --framework net8.0
```

## ?? Configuration

### Change Base URL
Edit `appsettings.json`:
```json
"BaseUrl": "http://localhost:5000/"
```

### Change Browser Type
Options: `"chromium"`, `"firefox"`, `"webkit"`
```json
"BrowserType": "chromium"
```

### Change Wait Timeout
```json
"ElementWaitTimeout": 10000   // in milliseconds
```

### Enable Video Recording
```json
"RecordVideo": true
```

### Enable Screenshots on Failure
```json
"CaptureScreenshot": true
```

## ?? Output Locations

| Output | Location |
|--------|----------|
| Logs | `logs/YYYY-MM-DD.log` |
| Screenshots | `screenshots/` |
| Videos | `videos/` |

## ?? Test Cases

### Test 1: Navigate All Pages
```powershell
dotnet test --filter "NavigateToAllPages_UsingMenu_ShouldSucceed"
```

### Test 2: Verify Invoice Summary
```powershell
dotnet test --filter "VerifyInvoiceSummaryTotal_ShouldMatchSumOfAllInvoices"
```

### Test 3: Verify Invoice I634
```powershell
dotnet test --filter "VerifyInvoiceAmount_I634_ShouldEqual_423_99_EUR"
```

## ??? Development

### Format Code
```powershell
dotnet format
```

### Restore Packages
```powershell
dotnet restore
```

### Add NuGet Package
```powershell
dotnet add package PackageName
```

## ?? Debugging

### Enable Verbose Logging
Logs are automatically saved to `logs/` folder

View latest log:
```powershell
Get-Content logs/* | Select-Object -Last 100
```

### View Failed Test Screenshots
```powershell
Get-ChildItem screenshots/ -Recurse
```

### Run with Console Output
```powershell
dotnet test --logger "console;verbosity=detailed" 2>&1 | Tee-Object -FilePath test-output.txt
```

## ?? Useful PowerShell Commands

### List all Test Methods
```powershell
dotnet test --list-tests
```

### Run Tests in Parallel (4 threads)
```powershell
dotnet test -- RunConfiguration.MaxCpuCount=4
```

### Run Tests Sequential (1 thread)
```powershell
dotnet test -- RunConfiguration.MaxCpuCount=1
```

## ?? Project Structure Quick Look

```
Configuration/
  ??? BrowserConfig.cs
  ??? TestSettings.cs

Core/
  ??? BaseTest.cs
  ??? DriverFactory.cs

PageObjects/
  ??? Pages/
  ?   ??? BasePage.cs
  ?   ??? MenuPage.cs
  ?   ??? InvoicesPage.cs
  ??? Models/
      ??? InvoiceRow.cs

Services/
  ??? InvoiceService.cs
  ??? NavigationService.cs

Tests/
  ??? Integration/
      ??? InvoiceTests.cs

Utilities/
  ??? Assertions.cs
  ??? Logger.cs
  ??? WaitHelper.cs
```

## ? Pre-Test Checklist

- [ ] Application running at `http://localhost:5000/`
- [ ] Playwright browsers installed: `pwsh bin/Debug/net8.0/playwright.ps1 install`
- [ ] Project builds successfully: `dotnet build`
- [ ] `appsettings.json` configured correctly
- [ ] .NET 8.0 installed: `dotnet --version`

## ?? Common Issues & Quick Fixes

| Issue | Solution |
|-------|----------|
| "Element not found" | Increase `ElementWaitTimeout` or verify CSS selector |
| "Browser won't launch" | Run `pwsh bin/Debug/net8.0/playwright.ps1 install` |
| "Connection refused" | Verify app is running at `http://localhost:5000/` |
| "Test timeout" | Increase `TimeoutMs` in `BrowserConfig` |
| "Summary row not found" | Check Invoices page HTML structure matches selector |

---

**Tip:** Copy-paste any command above into PowerShell to run it!
