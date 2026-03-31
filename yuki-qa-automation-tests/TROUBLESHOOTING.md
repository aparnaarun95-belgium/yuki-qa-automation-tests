# ?? Troubleshooting Guide

## Common Issues & Solutions

---

## 1. ? "Framework not found" / ".NET 3.1 not installed"

**Error:**
```
The framework 'Microsoft.NETCore.App', version '3.1.0' (arm64) was not found
```

**Cause:** Project was targeting .NET Core 3.1, but only .NET 8.0/9.0 are installed.

**Solution:** ? **ALREADY FIXED**
- Project has been upgraded to .NET 8.0
- Verify: `dotnet --version` should show 8.x.x or later

**Verify Fix:**
```powershell
dotnet build  # Should complete successfully
```

---

## 2. ? "Playwright browsers not found"

**Error:**
```
Executable doesn't exist at /path/to/chromium
```

**Cause:** Playwright browsers not installed

**Solution:**
```powershell
# Install Playwright browsers (one-time)
cd "C:\Users\arunk\Downloads\qa-automation-test\yuki-qa-automation-test\yuki-qa-automation-tests\yuki-qa-automation-tests"
pwsh bin/Debug/net8.0/playwright.ps1 install
```

**Verify:**
```powershell
# After installation, run tests
dotnet test
```

---

## 3. ? "Connection refused" / "Cannot reach localhost:5000"

**Error:**
```
System.Net.Http.HttpRequestException: Connection refused
```

**Cause:** Application not running at http://localhost:5000/

**Solution:**
1. Start your application
2. Verify it's accessible in browser: `http://localhost:5000/`
3. Update URL in `appsettings.json` if different:
   ```json
   "BaseUrl": "http://your-actual-url:port/"
   ```

**Verify:**
```powershell
# Test connectivity
Invoke-WebRequest -Uri "http://localhost:5000/" -UseBasicParsing
# Should return Status Code 200
```

---

## 4. ? "Element not found" / "Timeout waiting for selector"

**Error:**
```
PlaywrightException: Timeout 10000ms exceeded waiting for selector 'table.table tbody tr'
```

**Cause:** CSS selector doesn't match your HTML structure

**Solution:**

1. **Inspect your HTML** in browser developer tools (F12)
2. **Find the correct selector** for the element
3. **Update CSS selector** in page object

Example - if your table structure is different:
```csharp
// Old selector (wrong)
private const string TableRows = "table.table tbody tr";

// New selector (correct - adjust based on your HTML)
private const string TableRows = "table.invoice-table tbody tr";
```

4. **Increase wait timeout** in `appsettings.json`:
```json
"ElementWaitTimeout": 20000  // Increase from 10000
```

---

## 5. ? "Summary row not found"

**Error:**
```
Exception: Summary row not found
```

**Cause:** Invoice table doesn't have a summary row, or selector is wrong

**Solution:**

1. **Check your Invoice page HTML** - does it have a summary/footer row?
2. **Update selector** if needed in `InvoicesPage.cs`:
```csharp
// Current selector
private const string SummaryRow = "table.table tfoot tr";

// Or try
private const string SummaryRow = "table.table tr.summary";
```

3. **Or update the test** if summary row doesn't exist:
```csharp
// Modify test to skip summary verification
public async Task VerifyInvoiceSummaryTotal_ShouldMatchSumOfAllInvoices()
{
    // Check if summary row exists first
    var summaryExists = await _invoicesPage.Page.QuerySelectorAsync("table.table tfoot tr") != null;
    Assert.IsTrue(summaryExists, "Summary row should exist");
}
```

---

## 6. ? "Invoice I634 not found"

**Error:**
```
Exception: Invoice I634 not found
```

**Cause:** Invoice doesn't exist or selector doesn't match your data

**Solution:**

1. **Verify invoice exists** in your application
2. **Check invoice ID column** structure
3. **Update selector** if needed in `InvoicesPage.cs`:
```csharp
// Current selector (first column)
var invoiceIdElement = await row.QuerySelectorAsync("td:nth-child(1)");

// If ID is in different column, update:
var invoiceIdElement = await row.QuerySelectorAsync("td:nth-child(2)");  // Second column
```

4. **Run test with different invoice:**
```csharp
// Create new test with different invoice
const string invoiceId = "I001";  // Use an invoice that exists
const decimal expectedAmount = 100.00m;
```

---

## 7. ? Tests take too long / Timeouts

**Error:**
```
TimeoutException: Browser timeout after 30000ms
```

**Cause:** Slow application or network, short timeouts

**Solution:**

**Increase timeouts** in `appsettings.json`:
```json
{
  "TestSettings": {
    "PageLoadTimeout": 60000,      // Increase from 30000
    "ElementWaitTimeout": 20000,   // Increase from 10000
    "BrowserConfig": {
      "TimeoutMs": 60000           // Increase from 30000
    }
  }
}
```

---

## 8. ? "Invalid character in path"

**Error:**
```
ArgumentException: Invalid characters in path
```

**Cause:** Screenshot/log path contains invalid characters

**Solution:**

Update `appsettings.json` with valid path:
```json
{
  "TestSettings": {
    "BrowserConfig": {
      "ScreenshotPath": "./screenshots",  // Valid
      "VideoPath": "./videos"             // Valid
    }
  }
}
```

---

## 9. ? Tests fail silently / No output

**Error:**
```
No console output, tests seem to hang
```

**Cause:** Browser running in background, no visibility

**Solution:**

**Disable headless mode** in `appsettings.json`:
```json
"Headless": false  // You'll see browser open
```

**Or run with verbose logging:**
```powershell
dotnet test --logger "console;verbosity=detailed"
```

---

## 10. ? "File already in use" error

**Error:**
```
IOException: The process cannot access the file because it is being used by another process
```

**Cause:** Previous test session didn't close properly

**Solution:**

**Clean and rebuild:**
```powershell
dotnet clean
dotnet build
dotnet test
```

---

## ?? Debugging Steps

### Step 1: Verify Build
```powershell
dotnet build
```
? Should complete without errors

### Step 2: Check Configuration
```powershell
# Verify appsettings.json is valid JSON
Get-Content appsettings.json | ConvertFrom-Json
```

### Step 3: Check Logs
```powershell
# View latest logs
Get-ChildItem logs/ | Sort-Object LastWriteTime -Descending | Select-Object -First 1 | Get-Content -Tail 100
```

### Step 4: Run Single Test with Verbose Output
```powershell
dotnet test --filter "NavigateToAllPages_UsingMenu_ShouldSucceed" --logger "console;verbosity=detailed"
```

### Step 5: Enable Video Recording
```json
"RecordVideo": true
```
Then check `videos/` folder after test runs

---

## ?? Advanced Troubleshooting

### Check Playwright Version
```powershell
dotnet add package Microsoft.Playwright --version 1.40.0
```

### Reinstall Playwright Browsers
```powershell
# Complete fresh install
pwsh bin/Debug/net8.0/playwright.ps1 install --with-deps
```

### Test Connectivity
```powershell
# Test if app is reachable
Test-NetConnection -ComputerName localhost -Port 5000
```

### Check Port Availability
```powershell
# See what's running on port 5000
netstat -ano | findstr :5000
```

---

## ?? Diagnostics Checklist

Run through this if tests fail:

- [ ] .NET 8.0 installed? `dotnet --version`
- [ ] Playwright browsers installed? Try running install command again
- [ ] Application running? Check `http://localhost:5000/`
- [ ] appsettings.json valid JSON? Open in VS Code or JSON validator
- [ ] CSS selectors correct? Open F12 inspector and verify selectors
- [ ] Timeouts set reasonably? Check `appsettings.json`
- [ ] Recent changes to application? Verify page structure
- [ ] Recent Windows updates? May affect Playwright

---

## ?? Tips

1. **Use Browser Inspector** (F12) to:
   - Verify CSS selectors
   - Check element positions
   - Monitor network requests

2. **Enable Video Recording** to debug test failures:
   ```json
   "RecordVideo": true
   ```

3. **Check Logs** for detailed error messages:
   ```powershell
   Get-Content logs/YYYY-MM-DD.log | Select-Object -Last 50
   ```

4. **Run Tests Individually** while debugging:
   ```powershell
   dotnet test --filter "TestName"
   ```

5. **Disable Headless Mode** to watch browser actions:
   ```json
   "Headless": false
   ```

---

## ?? Still Having Issues?

1. **Check the logs**: `logs/YYYY-MM-DD.log`
2. **View screenshots**: `screenshots/` folder
3. **Verify selectors**: Use browser F12 inspector
4. **Test connectivity**: `Invoke-WebRequest http://localhost:5000/`
5. **Check .NET version**: `dotnet --version` should be 8.x or higher

---

**Good luck debugging! ??**
