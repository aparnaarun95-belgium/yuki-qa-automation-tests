# ? Getting Started Checklist

## Phase 1: Prerequisites (5 minutes)

- [ ] **Verify .NET 8.0 is installed**
  ```powershell
  dotnet --version
  # Expected: 8.0.x or higher
  ```

- [ ] **Navigate to project directory**
  ```powershell
  cd "C:\Users\arunk\Downloads\qa-automation-test\yuki-qa-automation-test\yuki-qa-automation-tests\yuki-qa-automation-tests"
  ```

- [ ] **Verify project file exists**
  ```powershell
  Get-Item yuki-qa-automation-tests.csproj
  # Should find the file
  ```

---

## Phase 2: Install Playwright (2 minutes)

- [ ] **Run Playwright installation (first time only)**
  ```powershell
  pwsh bin/Debug/net8.0/playwright.ps1 install
  # Wait for completion - this downloads browser binaries
  ```
  ?? This may take 2-5 minutes depending on internet speed

- [ ] **Verify Playwright installed**
  ```powershell
  # Try to run a simple test
  dotnet test --filter "NavigateToAllPages_UsingMenu_ShouldSucceed"
  ```

---

## Phase 3: Prepare Application (2 minutes)

- [ ] **Start your application**
  - Launch your app that's hosting at http://localhost:5000/

- [ ] **Verify application is running**
  - Open browser and navigate to http://localhost:5000/
  - Should see your application
  - Should have menu with: Home, Invoices, Customers, Products

- [ ] **Verify Invoices page**
  - Click on "Invoices" menu
  - Should see a table with invoices
  - Should have a summary row at bottom
  - Verify invoice "I634" exists with amount "423.99 EUR"

---

## Phase 4: Build Project (1 minute)

- [ ] **Build the project**
  ```powershell
  dotnet build
  # Expected: Build successful
  ```

- [ ] **Verify build succeeded**
  ```powershell
  # Should complete without errors
  # Check exit code: $? should be True
  ```

---

## Phase 5: Run Tests (5 minutes)

### Option A: Run All Tests
- [ ] **Execute all tests**
  ```powershell
  dotnet test
  ```

- [ ] **Wait for completion**
  - Browser will open automatically
  - You'll see test progress in console
  - Tests should complete in 30-60 seconds

- [ ] **Verify all tests passed**
  ```
  ? NavigateToAllPages_UsingMenu_ShouldSucceed
  ? VerifyInvoiceSummaryTotal_ShouldMatchSumOfAllInvoices
  ? VerifyInvoiceAmount_I634_ShouldEqual_423_99_EUR
  
  Passed: 3
  ```

### Option B: Run Tests Individually
- [ ] **Test 1: Navigation**
  ```powershell
  dotnet test --filter "NavigateToAllPages_UsingMenu_ShouldSucceed"
  ```

- [ ] **Test 2: Invoice Summary**
  ```powershell
  dotnet test --filter "VerifyInvoiceSummaryTotal_ShouldMatchSumOfAllInvoices"
  ```

- [ ] **Test 3: Specific Invoice**
  ```powershell
  dotnet test --filter "VerifyInvoiceAmount_I634_ShouldEqual_423_99_EUR"
  ```

---

## Phase 6: Review Output (2 minutes)

- [ ] **Check test results in console**
  - All tests should show ? (passed)
  - No errors should be present

- [ ] **Review test logs**
  ```powershell
  Get-ChildItem logs/ | Sort-Object LastWriteTime -Descending | Select-Object -First 1 | Get-Content
  # Should show detailed test execution
  ```

- [ ] **Verify folders created**
  ```powershell
  Get-ChildItem -Directory
  # Should see: bin, obj, logs, appsettings.json, etc.
  ```

---

## Phase 7: Troubleshooting (If needed)

If any test fails:

- [ ] **Check the error message**
  - Look at console output
  - Look at `logs/` folder

- [ ] **Check screenshots (if available)**
  ```powershell
  Get-ChildItem screenshots/
  ```

- [ ] **Verify CSS selectors match your app**
  - Open F12 browser inspector
  - Check if table.table, td elements exist
  - Compare with selectors in `PageObjects/Pages/InvoicesPage.cs`

- [ ] **See TROUBLESHOOTING.md for detailed help**

---

## ? Success Criteria

You'll know everything is working when:

- ? `dotnet build` completes without errors
- ? `dotnet test` runs without errors
- ? All 3 tests show as ? (Passed)
- ? Browser opens and navigates automatically
- ? `logs/` folder has today's date log file
- ? Test execution completes in under 2 minutes

---

## ?? After First Success

### Check these files:
```
? yuki-qa-automation-tests.csproj
? Configuration/BrowserConfig.cs
? Configuration/TestSettings.cs
? Core/BaseTest.cs
? Core/DriverFactory.cs
? PageObjects/Pages/BasePage.cs
? PageObjects/Pages/MenuPage.cs
? PageObjects/Pages/InvoicesPage.cs
? Services/InvoiceService.cs
? Services/NavigationService.cs
? Tests/Integration/InvoiceTests.cs
? Utilities/Logger.cs
? Utilities/WaitHelper.cs
? appsettings.json
```

### Review these guides:
- `SETUP_GUIDE.md` - Detailed information
- `QUICK_REFERENCE.md` - Commands reference
- `TROUBLESHOOTING.md` - Problem solving

---

## ?? Common Commands (After Setup)

```powershell
# Run all tests
dotnet test

# Run specific test
dotnet test --filter "TestName"

# Run with verbose output
dotnet test --logger "console;verbosity=detailed"

# List all tests
dotnet test --list-tests

# Clean build
dotnet clean
dotnet build

# View recent logs
Get-Content logs/* | Select-Object -Last 100
```

---

## ?? Timeline

| Phase | Duration | Completed |
|-------|----------|-----------|
| Prerequisites | 5 min | ? |
| Install Playwright | 2-5 min | ? |
| Prepare App | 2 min | ? |
| Build Project | 1 min | ? |
| Run Tests | 5 min | ? |
| Review Output | 2 min | ? |
| **Total** | **~20 min** | ? |

---

## ?? If Stuck

1. **First time issues?**
   - See: `SETUP_GUIDE.md`

2. **Need command help?**
   - See: `QUICK_REFERENCE.md`

3. **Tests failing?**
   - See: `TROUBLESHOOTING.md`

4. **General questions?**
   - See: `README.md`

5. **Want to add tests?**
   - See: `CONTRIBUTING.md`

---

## ? You're All Set!

Once you complete this checklist:

? Framework is installed
? Tests are configured
? Application is ready
? Everything is working

You can now:
- ? Run tests on demand
- ? Add new test cases
- ? Modify configurations
- ? Extend the framework

---

**Ready to begin? Start with Phase 1! ??**
