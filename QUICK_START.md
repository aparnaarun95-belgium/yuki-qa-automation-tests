# Quick Start Guide

Get up and running with the Yuki QA Automation Tests in 5 minutes!

## ? 5-Minute Setup

### 1. Clone & Install (1 minute)
```bash
git clone <repository-url>
cd yuki-qa-automation-tests
dotnet restore
```

### 2. Configure Test URL (1 minute)
Edit `appsettings.Development.json`:
```json
"BaseUrl": "http://localhost:5000/"  // Update to your test app URL
```

### 3. Run Tests (2 minutes)
```bash
# Local development (with UI)
set ASPNETCORE_ENVIRONMENT=Development
dotnet test

# Or CI/CD mode (headless)
set ASPNETCORE_ENVIRONMENT=Production
dotnet test
```

### 4. View Results (1 minute)
- **Test Output**: Console or terminal
- **Screenshots**: `./screenshots/` (on failure)
- **Logs**: `./logs/` (daily files)

## ?? Common Commands

```bash
# Run all tests
dotnet test

# Run specific test
dotnet test --filter="VerifyInvoiceAmount"

# Run with verbose output
dotnet test --logger:"console;verbosity=detailed"

# Run in parallel
dotnet test /p:ParallelizeAssembly=true

# Build only
dotnet build

# Clean build
dotnet clean
dotnet build
```

## ?? Configuration

### Local Development
**File**: `appsettings.Development.json`
```json
{
  "Headless": false,                    // See browser
  "PageLoadTimeout": 60000,             // 60 seconds
  "ElementWaitTimeout": 15000,          // 15 seconds
  "RetryAttempts": 1                    // Single try
}
```

### CI/CD Pipeline
**File**: `appsettings.json`
```json
{
  "Headless": true,                     // Fastest execution
  "PageLoadTimeout": 30000,             // 30 seconds
  "ElementWaitTimeout": 10000,          // 10 seconds
  "RetryAttempts": 3                    // Resilient
}
```

## ?? Writing Tests

### Test Template
```csharp
[Test]
public async Task YourTest_Scenario_ShouldExpected()
{
    // Arrange
    var expectedValue = "expected";

    // Act
    var result = await _service.GetValueAsync();

    // Assert
    Assert.AreEqual(expectedValue, result);
}
```

### Service Usage Example
```csharp
// In your test class
private InvoiceService _invoiceService;

[SetUp]
public new async Task Setup()
{
    await base.Setup();
    _invoiceService = new InvoiceService(_invoicesPage, Logger);
}

[Test]
public async Task TestInvoiceAmount()
{
    var result = await _invoiceService.VerifyInvoiceAmountAsync("I634", 423.99m);
    Assert.IsTrue(result);
}
```

## ??? Troubleshooting

### Issue: Tests Pass Locally but Fail in CI/CD
**Solution**: Check timeouts in `appsettings.json`
```json
{
  "PageLoadTimeout": 60000,      // Increase if needed
  "NavigationTimeout": 60000
}
```

### Issue: Browser Not Launching
**Solution**: Install Playwright browsers
```bash
pwsh bin/Debug/netcoreapp3.1/playwright.ps1 install
```

### Issue: Element Not Found
**Solution**: Increase wait timeout in test
```csharp
await WaitHelper.WaitForElementAsync(selector, 20000); // 20 seconds
```

### Issue: Out of Memory in Docker
**Solution**: Add to `BrowserConfig.Args` in config:
```json
"Args": ["--disable-dev-shm-usage", "--disable-gpu"]
```

## ?? Project Structure Quick Reference

```
??? Configuration/      ? App settings classes
??? Core/              ? Browser factory, base test
??? PageObjects/       ? Page interactions
??? Services/          ? Business logic
??? Tests/             ? Test cases (write here!)
??? Utilities/         ? Logging, waits, assertions
??? appsettings.json   ? CI/CD defaults
??? README.md          ? Full documentation
```

## ?? CI/CD Quick Start

### Azure Pipelines
1. Copy `azure-pipelines.yml` to repo root
2. Go to Azure DevOps > Pipelines > Create
3. Select existing YAML file
4. That's it! ??

### GitHub Actions
1. File `.github/workflows/playwright-tests.yml` auto-detects
2. Go to GitHub > Actions tab
3. See workflow in action ??

## ?? Key Files to Know

| File | Purpose |
|------|---------|
| `appsettings.json` | CI/CD configuration |
| `appsettings.Development.json` | Local dev configuration |
| `Services/InvoiceService.cs` | Business logic example |
| `PageObjects/Pages/InvoicesPage.cs` | Page interaction example |
| `Tests/Integration/InvoiceTests.cs` | Test examples |

## ?? Pro Tips

1. **Enable debug logging**: `set CI=true` then `dotnet test`
2. **Save videos on failure**: Set `RecordVideo: true` in config
3. **Add slow motion**: Set `SlowMo: 1000` for debugging
4. **Run one test**: `dotnet test --filter="TestName"`
5. **Check logs**: `logs/2024-01-15.log` (daily files)

## ?? Need Help?

1. Check `README.md` (full documentation)
2. Review `CONTRIBUTING.md` (architecture guide)
3. See `CI-CD_SETUP_GUIDE.md` (pipeline setup)
4. Check `logs/` directory for error details
5. Review `screenshots/` for visual issues

## ?? Next Steps

- [ ] Review `README.md` for detailed documentation
- [ ] Check `CONTRIBUTING.md` for coding standards
- [ ] Run tests locally to verify setup
- [ ] Setup CI/CD pipeline using `CI-CD_SETUP_GUIDE.md`
- [ ] Write your first test following the template

---

Happy Testing! ??
