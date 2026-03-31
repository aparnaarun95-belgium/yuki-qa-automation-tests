# ?? Where to Find Test Results

## Quick Answer

Test results appear in **4 main locations** depending on how you run tests:

1. **Console Output** - Immediate results
2. **Screenshots/** - Failure screenshots
3. **TestResults/** - XML test reports
4. **Visual Studio Test Explorer** - GUI interface

---

## 1. Console Output (Immediate Results)

### **Running Tests**
```bash
dotnet test
```

### **What You'll See**
```
Test run for C:\...\yuki-qa-automation-tests.dll (.NET 8.0)
Microsoft (R) Test Execution Command Line Tool Version 17.8.0

Starting test execution, please wait...
A total of 1 test files to be run

Passed  TC001_VerifyHomePageLoads [123 ms]
Passed  TC002_VerifyWelcomeHeadingVisible [145 ms]
Failed  TC003_VerifySignInButtonVisible [2543 ms]
Skipped TC004_VerifySignUpButtonVisible

Test run finished - 2 passed, 1 failed, 1 skipped, took 2.8 seconds
```

### **Viewing with Different Detail Levels**

**Minimal Output:**
```bash
dotnet test
```

**Normal Output (Recommended):**
```bash
dotnet test -v normal
```

**Detailed Output:**
```bash
dotnet test -v d
```

**Quiet Output:**
```bash
dotnet test -v q
```

---

## 2. Screenshots Folder (On Test Failure)

### **Location**
```
yuki-qa-automation-tests/Screenshots/
```

### **File Naming Convention**
```
TestName_yyyyMMdd_HHmmss_FAILURE.png
```

### **Example**
```
TC003_VerifySignInButtonVisible_20240315_143022_FAILURE.png
TC005_VerifyNavigationMenuPresent_20240315_144530_FAILURE.png
```

### **What They Contain**
- Full page screenshot at moment of failure
- Shows actual page state
- Helps debug why test failed

### **When They're Created**
- Automatically when test fails
- Only if test status ? Passed
- Folder created automatically on first failure

### **How to View**
1. Navigate to `Screenshots/` folder
2. Double-click `.png` file
3. Opens in default image viewer

---

## 3. TestResults Folder (XML Reports)

### **Generate Results File**
```bash
dotnet test --logger trx
```

### **Location**
```
yuki-qa-automation-tests/TestResults/
```

### **File Format**
```
TestResults_*.trx
```

### **Example**
```
TestResults_COMPUTERNAME_2024-03-15_14_30_22.trx
```

### **How to View in Visual Studio**
1. Open `Test Explorer` (Ctrl+E, T)
2. Click "Open Test Results File"
3. Select `.trx` file from `TestResults/` folder
4. View detailed results with graphs

### **How to Open Directly**
1. Navigate to `TestResults/` folder
2. Right-click `.trx` file
3. Select "Open With"
4. Choose "Visual Studio"

---

## 4. Visual Studio Test Explorer (GUI)

### **Open Test Explorer**
```
Menu: Test > Test Explorer
OR
Keyboard: Ctrl + E, T
```

### **What You See**
- List of all tests
- Color-coded status (Green=Pass, Red=Fail, Yellow=Skip)
- Execution time for each test
- Grouping by Class/Category

### **Running Tests from Explorer**
```
• Click "Run All Tests" (green play icon)
• Or right-click specific test > "Run"
• Or right-click test class > "Run Tests"
```

### **Features**
- **Play** - Run selected tests
- **Stop** - Cancel running tests
- **Reload** - Refresh test list
- **Filter** - Search tests by name
- **Group By** - Organize by Class/Category/Outcome

### **Viewing Details**
1. Run tests
2. Click test name to see details
3. View output panel at bottom
4. See execution time and error messages

---

## Complete Test Run Command Examples

### **Example 1: Quick Console Test**
```bash
dotnet test
```
**Result:** Console output only, ~2 seconds

### **Example 2: Console + Detailed Logging**
```bash
dotnet test -v normal
```
**Result:** Detailed console output with all steps logged

### **Example 3: With Test Results File**
```bash
dotnet test --logger trx
```
**Result:** Console output + `TestResults/*.trx` file created

### **Example 4: Smoke Tests Only**
```bash
dotnet test --filter "Category=Smoke"
```
**Result:** Only TC001-TC010 run

### **Example 5: Specific Test**
```bash
dotnet test --filter "Name=TC001_VerifyHomePageLoads"
```
**Result:** Only that specific test runs

### **Example 6: Run with Screenshots**
```bash
dotnet test
```
**Result:** On any failure, screenshot saved to `Screenshots/` folder

---

## Artifact Locations Summary

| Item | Location | Created | Format |
|------|----------|---------|--------|
| **Console Output** | Terminal | Always | Text |
| **Screenshots** | `Screenshots/` | On Failure | PNG |
| **Test Results** | `TestResults/` | With `--logger trx` | TRX (XML) |
| **Test Explorer** | Visual Studio | Always | UI |

---

## Step-by-Step: First Test Run

### **Step 1: Open Terminal**
```bash
cd yuki-qa-automation-tests
```

### **Step 2: Run Tests**
```bash
dotnet test
```

### **Step 3: Watch Console Output**
You'll see:
- Test class names
- Test method names (TC###)
- Pass/Fail status
- Execution time
- Summary: X passed, Y failed, Z skipped

### **Step 4: Check for Screenshots (if any failed)**
```bash
ls Screenshots/
```

### **Step 5: Generate Report File (Optional)**
```bash
dotnet test --logger trx
```

### **Step 6: View in Visual Studio (Optional)**
- Open Test Explorer: `Ctrl + E, T`
- Click on test results

---

## Reading Test Output

### **Passed Test Example**
```
Passed  TC001_VerifyHomePageLoads [123 ms]
? Green checkmark
? Shows test name
? Shows execution time
```

### **Failed Test Example**
```
Failed  TC003_VerifySignInButtonVisible [2543 ms]
System.AssertionException: Expected element to be visible
    at HomePage.IsSignInButtonVisibleAsync()
    at HomePageTests.TC003_VerifySignInButtonVisible()

Screenshot: Screenshots/TC003_VerifySignInButtonVisible_20240315_143022_FAILURE.png
```

### **Skipped Test Example**
```
Skipped TC004_VerifySignUpButtonVisible
[Skip] Not implemented yet
```

---

## Common Viewing Scenarios

### **Scenario 1: "I want to see if tests passed"**
```bash
dotnet test
```
? Check console for PASSED/FAILED counts

### **Scenario 2: "Why did test fail?"**
```bash
dotnet test -v normal
```
? Read detailed error in console
? View screenshot in `Screenshots/`

### **Scenario 3: "I want detailed metrics"**
```bash
dotnet test --logger trx
```
? Open `TestResults/*.trx` in Visual Studio
? View graphs and detailed metrics

### **Scenario 4: "I want GUI interface"**
```
In Visual Studio:
Ctrl + E, T  // Open Test Explorer
Click "Run All"
```

### **Scenario 5: "I want to debug a specific test"**
```
In Visual Studio Test Explorer:
Right-click test ? "Debug"
```

---

## Creating a Test Results Summary

### **Script: View Latest Results**
```bash
# Show last 10 test results
dotnet test --logger trx 2>&1 | tail -20
```

### **Script: Find Failed Tests**
```bash
# List all failures
dotnet test --filter "TestCategory=UI" -v normal 2>&1 | grep "Failed"
```

### **Script: Show Test Timings**
```bash
# Run and show execution times
dotnet test -v normal 2>&1 | grep "\[.*ms\]"
```

---

## Key Files to Check After Running Tests

| File/Location | Check For | What It Tells You |
|---|---|---|
| **Console Output** | "X passed, Y failed" | Overall test status |
| **Screenshots/** | PNG files | Why tests failed |
| **TestResults/*.trx** | Details section | Detailed metrics |
| **Test Explorer** | Red/Green indicators | Visual pass/fail status |

---

## Tips & Tricks

### **Tip 1: Quick Pass/Fail Check**
```bash
dotnet test 2>&1 | grep "passed\|failed"
```

### **Tip 2: Save Results to File**
```bash
dotnet test > test_results.txt 2>&1
```
Then view: `test_results.txt`

### **Tip 3: Run Fastest Tests First**
```bash
dotnet test --filter "Category=Smoke" -- NUnit.NumberOfTestWorkers=4
```

### **Tip 4: Capture Specific Test Details**
```bash
dotnet test --filter "Name=TC001*" -v normal
```

### **Tip 5: View Screenshots in Folder**
```bash
explorer Screenshots/
```
(Windows Explorer opens, showing all failure screenshots)

---

## Visual Studio Test Explorer Features

### **Group Results By:**
- **Class** - Tests grouped by test class
- **Category** - Tests grouped by category (Smoke, UI, etc.)
- **Outcome** - Tests grouped by Pass/Fail/Skip
- **Traits** - Custom grouping

### **Filter Results By:**
- Test name
- Category
- Outcome (passed/failed)
- Execution time

### **Run Specific Tests:**
- Click "Run" on individual test
- Right-click and select "Run Tests"
- Select multiple tests and run together

---

## Example: Complete Test Run Output

```
Starting test execution, please wait...
A total of 1 test files to be run

Passed  TC001_VerifyHomePageLoads [123 ms]
Passed  TC002_VerifyWelcomeHeadingVisible [145 ms]
Passed  TC003_VerifySignInButtonVisible [98 ms]
Passed  TC004_VerifySignUpButtonVisible [156 ms]
Passed  TC005_VerifyNavigationMenuPresent [187 ms]
Passed  TC006_VerifySearchFunctionalityPresent [203 ms]
Passed  TC007_VerifyFeaturedSectionVisible [89 ms]
Passed  TC008_VerifyPageTitle [67 ms]
Passed  TC009_VerifyPageResponsiveness [145 ms]
Passed  TC010_VerifyPageLoadTime [234 ms]
Passed  TC011_VerifySignInButtonClickable [156 ms]
Passed  TC012_VerifySignUpButtonClickable [123 ms]
Passed  TC013_VerifySearchFunctionality [198 ms]
Passed  TC014_VerifySearchWithMultipleTerms(Feature) [145 ms]

Test run finished - 14 passed, 0 failed, 0 skipped, took 2.5 seconds
```

---

## Summary

? **Console** - Quickest way to see results
? **Screenshots/** - Visual debugging of failures
? **TestResults//** - Detailed metrics and reports
? **Test Explorer** - GUI interface in Visual Studio
? **Command Line** - Full control over what to run

**Choose based on your need:**
- **Quick check** ? Console output
- **Debugging failure** ? Screenshots
- **Metrics & reporting** ? Test Results file
- **Visual interface** ? Test Explorer
