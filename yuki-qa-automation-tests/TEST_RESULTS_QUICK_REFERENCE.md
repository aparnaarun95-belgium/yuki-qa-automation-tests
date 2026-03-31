# ?? Quick Reference: Test Results Locations

## Where to Find Test Results - Quick Map

```
???????????????????????????????????????????????????????????????????
?              TEST RESULTS VIEWING OPTIONS                        ?
???????????????????????????????????????????????????????????????????

?
?? ?? CONSOLE OUTPUT (Immediate)
?  ?? Command: dotnet test
?  ?? Shows: PASSED ?, FAILED ?, execution times
?  ?? Speed: Instant
?
?? ?? SCREENSHOTS FOLDER (On Failure)
?  ?? Location: ./Screenshots/
?  ?? Files: TestName_yyyyMMdd_HHmmss_FAILURE.png
?  ?? Shows: Full page screenshots of failures
?  ?? When: Auto-created on test failure
?
?? ?? TEST RESULTS FILE (Detailed Report)
?  ?? Command: dotnet test --logger trx
?  ?? Location: ./TestResults/
?  ?? Files: TestResults_*.trx (XML format)
?  ?? Shows: Detailed metrics, graphs
?  ?? View: In Visual Studio
?
?? ???  TEST EXPLORER GUI (Visual Studio)
   ?? Shortcut: Ctrl + E, T
   ?? Shows: All tests with color-coded status
   ?? Features: Filter, group, run tests
   ?? Best for: Visual overview
```

---

## Quick Commands

```bash
# ? View Results in Console (FASTEST)
dotnet test

# ? View Detailed Console Output
dotnet test -v normal

# ? Generate & View Test Report
dotnet test --logger trx

# ? Run Only Smoke Tests (Fastest)
dotnet test --filter "Category=Smoke"

# ? Run Specific Test
dotnet test --filter "Name=TC001*"

# ? Save Results to File
dotnet test > results.txt
```

---

## Test Result Locations

| Location | Format | How to View | When Created |
|----------|--------|------------|--------------|
| **Console** | Text | `dotnet test` | Always |
| **Screenshots/** | PNG images | File explorer | On failure |
| **TestResults/** | XML (TRX) | VS or `--logger trx` | With `--logger` |
| **Test Explorer** | GUI | VS Ctrl+E,T | Always |

---

## Examples

### Example 1: Run Tests & See Results Immediately
```bash
$ dotnet test

Passed  TC001_VerifyHomePageLoads [123 ms]
Passed  TC002_VerifyWelcomeHeadingVisible [145 ms]
Failed  TC003_VerifySignInButtonVisible [2543 ms]

Test run finished - 2 passed, 1 failed
```

### Example 2: Check Failure Screenshot
```bash
$ ls Screenshots/
TC003_VerifySignInButtonVisible_20240315_143022_FAILURE.png

$ # Double-click the PNG file to open in image viewer
```

### Example 3: Generate Test Report
```bash
$ dotnet test --logger trx
$ # Opens file: TestResults/TestResults_COMPUTERNAME_2024-03-15_14_30_22.trx
$ # View in Visual Studio > Test Explorer > Open Test Results File
```

---

## Reading Test Output

```
? PASSED                           - Test succeeded
? FAILED                            - Test failed (check Screenshots/)
? SKIPPED                           - Test skipped ([Skip] attribute)
[123 ms]                            - Execution time
Test run finished - 14 passed...    - Summary line
```

---

## Visual Studio Test Explorer

### Open It
- Menu: `Test > Test Explorer`
- Keyboard: `Ctrl + E, T`

### Use It
1. Tests list on left
2. Run buttons at top
3. Results on bottom
4. Details panel shows test output

### Color Coding
- ?? **Green** = PASSED
- ?? **Red** = FAILED
- ?? **Yellow** = SKIPPED

---

## Pro Tips

?? **For CI/CD:** Use `dotnet test` (console only, fastest)
?? **For Debugging:** Check `Screenshots/` folder for failure images
?? **For Reporting:** Generate `--logger trx` for detailed reports
?? **For Development:** Use Test Explorer GUI for interactive testing
?? **For Quick Check:** Run `dotnet test 2>&1 | grep "passed\|failed"`

---

## File Paths Reference

```
yuki-qa-automation-tests/
??? Screenshots/                    ? Failure screenshots
?   ??? TC003_..._FAILURE.png
??? TestResults/                    ? Test report files
?   ??? TestResults_*.trx
??? Tests/                          ? Your test files
    ??? HomePageTests.cs
    ??? SignInTests.cs
    ??? DataDrivenTests.cs
```

---

## Most Common: View Results After Running

```bash
# 1. Run tests
dotnet test

# 2. Results show immediately in console
# 3. If any failed, check Screenshots/ folder
# 4. Open with image viewer to debug

# Done! ?
```

---

**Start with:** `dotnet test` in your terminal
**Results appear:** Immediately in console ?
