# Contributing to Yuki QA Automation Tests

Thank you for your interest in contributing to this test automation framework! This document provides guidelines and best practices for contributing.

## ?? Table of Contents
- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Guidelines](#development-guidelines)
- [Commit Guidelines](#commit-guidelines)
- [Pull Request Process](#pull-request-process)

## ?? Code of Conduct

We are committed to providing a welcoming and inclusive environment. Please:
- Be respectful and professional
- Welcome newcomers and help them learn
- Focus on constructive feedback
- Report incidents to project maintainers

## ?? Getting Started

### Prerequisites
- .NET Core 3.1 or higher
- Git
- Playwright browsers installed

### Local Setup
```bash
git clone <repository-url>
cd yuki-qa-automation-tests
dotnet restore
```

### Verify Setup
```bash
# Run tests to verify everything works
dotnet test

# Or run specific test
dotnet test --filter="NavigateToAllPages"
```

## ?? Development Guidelines

### Code Style

Follow these C# conventions:

1. **Naming**: Use PascalCase for classes/methods, camelCase for variables
   ```csharp
   public class InvoiceService { }
   public async Task<List<InvoiceRow>> GetAllInvoicesAsync() { }
   private string _invoiceId;
   ```

2. **Documentation**: Include XML comments for public members
   ```csharp
   /// <summary>
   /// Verifies invoice amount matches expected value
   /// </summary>
   /// <param name="invoiceId">The invoice identifier</param>
   /// <param name="expectedAmount">The expected amount in EUR</param>
   /// <returns>True if amounts match, false otherwise</returns>
   public async Task<bool> VerifyInvoiceAmountAsync(string invoiceId, decimal expectedAmount)
   ```

3. **Async/Await**: Always use async/await for I/O operations
   ```csharp
   // ? Good
   public async Task NavigateAsync(string url)
   {
       await Page.GotoAsync(url);
   }
   
   // ? Avoid
   public void Navigate(string url)
   {
       Page.GotoAsync(url).Wait();
   }
   ```

4. **Error Handling**: Use try-catch with proper logging
   ```csharp
   try
   {
       await _page.ClickAsync(selector);
   }
   catch (PlaywrightException ex)
   {
       _logger.Error($"Failed to click element '{selector}': {ex.Message}");
       throw;
   }
   ```

5. **Null Checking**: Use null-coalescing operators
   ```csharp
   // ? Good
   public PageService(IPage page, Logger logger)
   {
       _page = page ?? throw new ArgumentNullException(nameof(page));
       _logger = logger ?? throw new ArgumentNullException(nameof(logger));
   }
   ```

### Architecture Patterns

#### 1. Page Object Model
Create page objects for each page/component:

```csharp
public class InvoicesPage : BasePage
{
    private const string InvoiceTable = "table.table";
    private const string TableRows = "table.table tbody tr";

    public InvoicesPage(IPage page, string baseUrl, Logger logger) 
        : base(page, baseUrl, logger)
    {
    }

    public async Task<List<InvoiceRow>> GetAllInvoices()
    {
        await WaitHelper.WaitForElementAsync(InvoiceTable);
        // Implementation...
    }
}
```

#### 2. Service Layer
Encapsulate business logic in services:

```csharp
public class InvoiceService
{
    private readonly InvoicesPage _invoicesPage;
    private readonly Logger _logger;

    public InvoiceService(InvoicesPage invoicesPage, Logger logger)
    {
        _invoicesPage = invoicesPage ?? throw new ArgumentNullException(nameof(invoicesPage));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> VerifySummaryTotalAsync()
    {
        var invoices = await _invoicesPage.GetAllInvoices();
        var calculatedTotal = invoices.Sum(i => i.Amount);
        var summaryTotal = await _invoicesPage.GetSummaryTotal();
        return summaryTotal == calculatedTotal;
    }
}
```

#### 3. Test Structure
Follow the Arrange-Act-Assert pattern:

```csharp
[Test]
public async Task VerifyInvoiceAmount_I634_ShouldEqual_423_99_EUR()
{
    // Arrange
    await _invoicesPage.NavigateToInvoicesPage();
    const string invoiceId = "I634";
    const decimal expectedAmount = 423.99m;

    // Act
    var result = await _invoiceService.VerifyInvoiceAmountAsync(invoiceId, expectedAmount);

    // Assert
    Assert.IsTrue(result, $"Invoice {invoiceId} amount should be {expectedAmount} EUR");
}
```

### Performance Considerations

1. **Timeouts**: Use appropriate timeouts for different operations
   ```csharp
   // Navigation might take longer
   await WaitHelper.WaitForPageLoadAsync(60000);
   
   // Element wait should be shorter
   await WaitHelper.WaitForElementAsync(selector, 10000);
   ```

2. **Resource Optimization**: Block unnecessary resources
   ```csharp
   // In DriverFactory
   await page.RouteAsync("**/*.{png,jpg,jpeg,gif,svg,webp,woff,woff2}", 
       route => route.AbortAsync());
   ```

3. **Parallel Execution**: Design tests to be independent
   - Avoid shared state
   - Use unique identifiers for test data
   - Clean up after each test

### CI/CD Considerations

1. **Environment Configuration**: Support environment-specific settings
   ```csharp
   var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
   ```

2. **Logging**: Enable detailed logging in CI/CD
   ```csharp
   if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI")))
   {
       _debugEnabled = true;
   }
   ```

3. **Retry Logic**: Implement retry for transient failures
   ```csharp
   for (int attempt = 0; attempt < TestSettings.RetryAttempts; attempt++)
   {
       try { /* operation */ }
       catch when (attempt < TestSettings.RetryAttempts - 1)
       {
           await Task.Delay(TestSettings.RetryDelayMs);
       }
   }
   ```

## ?? Commit Guidelines

### Commit Messages

Use clear, descriptive commit messages:

```
[FEATURE] Add invoice verification service
[FIX] Handle timeout exceptions in WaitHelper
[REFACTOR] Extract browser initialization logic
[DOCS] Update README with CI/CD instructions
[TEST] Add retry logic tests
[CHORE] Update dependencies
```

### Examples

- ? `[FEATURE] Implement parallel test execution support`
- ? `[FIX] Handle null reference in page navigation`
- ? `[DOCS] Add troubleshooting section to README`
- ? `Fix bug` (too vague)
- ? `Update code` (non-descriptive)

## ?? Pull Request Process

### Before Creating a PR

1. **Create a branch** from `main`:
   ```bash
   git checkout -b feature/invoice-verification
   ```

2. **Make your changes** following guidelines above

3. **Test locally**:
   ```bash
   dotnet test
   dotnet test --filter="YourNewTest"
   ```

4. **Run in both configurations**:
   ```bash
   set ASPNETCORE_ENVIRONMENT=Development
   dotnet test
   
   set ASPNETCORE_ENVIRONMENT=Production
   dotnet test
   ```

### PR Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Related Issue
Closes #(issue number)

## Testing
- [ ] Verified locally
- [ ] Tested in Development environment
- [ ] Tested in CI/CD configuration
- [ ] Added/updated tests

## Checklist
- [ ] Code follows style guidelines
- [ ] XML documentation added
- [ ] Tests pass locally
- [ ] No new warnings introduced
- [ ] README updated if needed
```

### Review Process

1. **CI/CD Checks** must pass:
   - Build succeeds
   - All tests pass
   - No code quality issues

2. **Code Review** considerations:
   - Does it follow our patterns?
   - Is it performant?
   - Will it work in CI/CD?
   - Is it well-documented?

3. **Merge Criteria**:
   - At least one approval
   - All checks passing
   - Conflicts resolved

## ?? Adding New Tests

### Test Naming Convention
```csharp
[Test]
public async Task <Action>_<Scenario>_Should<ExpectedResult>()
```

Examples:
- `VerifyInvoiceAmount_WithValidInvoiceId_ShouldReturnCorrectAmount`
- `NavigateToAllPages_UsingMenu_ShouldSucceed`
- `GetInvoices_WhenPageEmpty_ShouldReturnEmptyList`

### Test Template
```csharp
/// <summary>
/// Verify behavior under specific condition
/// </summary>
[Test]
public async Task YourTest_Scenario_ShouldBehavior()
{
    // Arrange
    var expectedValue = "expectedValue";

    // Act
    var result = await _service.GetValueAsync();

    // Assert
    Assert.AreEqual(expectedValue, result, "Clear assertion message");
}
```

## ?? Bug Reports

When reporting bugs, please include:

1. **Environment**:
   - OS (Windows/Linux/macOS)
   - .NET version
   - Browser type

2. **Steps to Reproduce**:
   ```
   1. Configure X
   2. Run test Y
   3. Observe Z
   ```

3. **Expected vs. Actual**:
   - What should happen
   - What actually happened

4. **Logs/Screenshots**:
   - Screenshots from `./screenshots/`
   - Relevant log entries
   - Error messages

## ?? Resources

- [Playwright Documentation](https://playwright.dev/dotnet/)
- [NUnit Documentation](https://docs.nunit.org/)
- [C# Coding Guidelines](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [Async/Await Best Practices](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)

## ? Questions?

Feel free to open an issue or reach out to the project maintainers.

---

Thank you for contributing! ??