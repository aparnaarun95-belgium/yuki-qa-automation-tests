# Test Failure Diagnosis Guide

## Issues That Could Cause Test Failures

### 1. **Application Not Running**
- **Symptom**: Tests timeout or fail immediately when navigating to `http://localhost:5000`
- **Fix**: Ensure your web application is running on `http://localhost:5000`
- **Debug**: Check the Output window for "Navigation failed" messages

### 2. **Element Selectors Don't Match**
- **Symptom**: Tests fail on assertions checking element visibility (TC002, TC003, TC004, TC005, TC006, TC007, TC008)
- **Possible Causes**:
  - Page elements don't have the expected CSS selectors
  - Element text doesn't match `has-text()` patterns
  - Missing `data-testid` attributes
- **Fix**: Inspect your application's HTML and update selectors in `HomePage.cs`:
  ```csharp
  private const string SignInButton = "[data-testid='sign-in-button'], button:has-text('Sign In')";
  ```
- **Debug**: Check Visual Studio Output window for Debug messages showing which selectors failed

### 3. **Network/Load State Issues**
- **Symptom**: Tests timeout waiting for `NetworkIdle` state
- **Possible Causes**:
  - Application has continuous background requests
  - JavaScript keeps network active
- **Fix**: Try using `WaitUntilState.DOMContentLoaded` instead of `NetworkIdle` in `HomePage.NavigateToAsync()`
- **Debug**: Increase timeouts temporarily to see if content eventually loads

### 4. **Element Visibility Timing**
- **Symptom**: Elements exist but aren't visible within timeout period
- **Possible Causes**:
  - JavaScript animations/delays before showing elements
  - CSS `display: none` or `visibility: hidden`
- **Fix**: Increase `DefaultTimeout` in `BaseTest.cs` from 30 seconds or adjust individual test timeouts
- **Debug**: Look at application console for errors during page load

### 5. **Test-Specific Issues**

#### TC001_VerifyHomePageLoads ? FIXED
- Now correctly checks for `localhost:5000` in URL

#### TC002_VerifyWelcomeHeadingVisible
- **May fail if**: No `h1` or `[data-testid='welcome-heading']` exists
- **Fix**: Update `WelcomeHeading` selector to match your page

#### TC003-TC004_VerifySignInSignUpButtonVisible
- **May fail if**: Buttons don't have `Sign In`/`Sign Up` text
- **Fix**: Update `SignInButton` and `SignUpButton` selectors

#### TC005_VerifyNavigationMenuPresent
- **May fail if**: No `<nav>` element or `[data-testid='navigation']`
- **Fix**: Update `NavMenu` selector

#### TC006_VerifySearchFunctionalityPresent
- **May fail if**: No search input with matching selector
- **Fix**: Update `SearchBox` selector

#### TC007_VerifyFeaturedSectionVisible
- **May fail gracefully** (has conditional assertion)
- **Fix**: Update `FeaturedSection` selector if section should exist

#### TC008_VerifyPageTitle
- **May fail if**: Page title is empty
- **Check**: Application sets proper `<title>` tag

#### TC009_VerifyPageResponsiveness
- **May fail if**: Page doesn't load or viewport isn't set
- **Check**: Ensure base page loads successfully

#### TC010_VerifyPageLoadTime
- **May fail if**: Page takes longer than 10 seconds to load
- **Fix**: Increase timeout or optimize application performance

## How to Diagnose Failures

### Step 1: Check Application Status
```powershell
# Verify localhost:5000 is accessible
curl http://localhost:5000
```

### Step 2: Check Visual Studio Output Window
- View ? Output (or Ctrl+Alt+O)
- Look for "Debug" pane messages showing which selectors failed
- Example: `Element not visible - Selector: [data-testid='sign-in-button'], Error: Timeout 30000ms exceeded`

### Step 3: Enable Additional Logging
Add to test setup:
```csharp
[SetUp]
public async Task TestSetup()
{
    _homePage = new HomePage(Page, DefaultTimeout);
    BaseUrl = "http://localhost:5000";

    // Enable detailed logging
    Page.Request += (_, request) => 
        TestContext.WriteLine($"? Request: {request.Method} {request.Url}");
    Page.Response += (_, response) => 
        TestContext.WriteLine($"? Response: {response.Status} {response.Url}");
}
```

### Step 4: Run Single Test with Debugging
1. Set breakpoint in test
2. Run ? Debug Selected Tests (Right-click on test)
3. Step through and inspect page content with:
   ```csharp
   var pageContent = await _homePage.GetPageContentAsync();
   TestContext.WriteLine(pageContent);
   ```

### Step 5: Update Selectors Based on Actual Page
Inspect your application HTML and update `HomePage.cs` selectors accordingly.

## Updated Improvements Made

1. **Better Selector Prioritization**: Now checks `data-testid` first, then fallback selectors
2. **Debug Logging**: Added detailed error logging to identify failing selectors
3. **Navigation Error Logging**: Added error reporting to navigation method

Run tests again and check the Output window for detailed diagnostic information.
