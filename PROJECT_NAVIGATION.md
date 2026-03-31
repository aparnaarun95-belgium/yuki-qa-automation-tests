# ?? Project Structure & Navigation Guide

## Directory Organization

```
yuki-qa-automation-tests/
?
??? ?? Configuration/                    (Configuration Management)
?   ??? BrowserConfig.cs                 ? Browser settings
?   ??? TestSettings.cs                  ? Test configuration
?
??? ?? Core/                             (Framework Infrastructure)
?   ??? BaseTest.cs                      ? Base test class with lifecycle
?   ??? DriverFactory.cs                 ? Browser factory
?
??? ?? PageObjects/                      (Page Object Model)
?   ??? ?? Pages/
?   ?   ??? BasePage.cs                  ? Common page methods
?   ?   ??? InvoicesPage.cs              ? Invoice interactions
?   ?   ??? MenuPage.cs                  ? Menu navigation
?   ??? ?? Models/
?       ??? InvoiceRow.cs                ? Data model
?
??? ?? Services/                         (Business Logic Layer)
?   ??? InvoiceService.cs                ? Invoice operations
?   ??? NavigationService.cs             ? Navigation logic
?
??? ?? Tests/Integration/                (Test Cases)
?   ??? InvoiceTests.cs                  ? Test implementations
?
??? ?? Utilities/                        (Helper Classes)
?   ??? Logger.cs                        ? Logging utility
?   ??? WaitHelper.cs                    ? Wait strategies
?   ??? Assertions.cs                    ? Custom assertions
?
??? ?? .github/workflows/                (GitHub Actions)
?   ??? playwright-tests.yml             ? CI workflow
?
??? ?? logs/                             (Generated at runtime)
?   ??? 2024-01-15.log                   ? Daily log files
?
??? ?? screenshots/                      (Generated on failure)
?   ??? TestName_timestamp.png           ? Failure screenshots
?
??? ?? appsettings.json                  (CI/CD Configuration)
??? ?? appsettings.Development.json      (Dev Configuration)
?
??? ?? azure-pipelines.yml               (Azure Pipelines)
??? ?? .gitignore                        (Git ignore rules)
?
??? ?? README.md                         (Main documentation)
??? ?? CONTRIBUTING.md                   (Development guide)
??? ?? CI-CD_SETUP_GUIDE.md             (Pipeline setup)
??? ?? QUICK_START.md                    (5-minute setup)
??? ?? IMPLEMENTATION_SUMMARY.md         (What was done)
??? ?? FINAL_STATUS.md                   (Project status)
?
??? ?? yuki-qa-automation-tests.csproj   (Project file)
```

## ?? Where to Find What

### For Getting Started
- **Quick Setup**: `QUICK_START.md` (5 minutes)
- **Full Documentation**: `README.md` (comprehensive)
- **Development Rules**: `CONTRIBUTING.md` (code standards)

### For CI/CD Setup
- **All Platforms**: `CI-CD_SETUP_GUIDE.md` (step-by-step)
- **Azure Pipelines**: `azure-pipelines.yml` + guide
- **GitHub Actions**: `.github/workflows/playwright-tests.yml` + guide

### For Configuration
- **CI/CD Settings**: `appsettings.json`
- **Local Dev Settings**: `appsettings.Development.json`
- **Browser Settings**: `Configuration/BrowserConfig.cs`

### For Adding Tests
- **Test Template**: `Tests/Integration/InvoiceTests.cs`
- **Page Objects**: `PageObjects/Pages/*.cs`
- **Services**: `Services/InvoiceService.cs`

### For Debugging
- **Logs**: `logs/` directory (daily files)
- **Screenshots**: `screenshots/` directory (on failure)
- **Test Output**: Console output during `dotnet test`

---

## ?? Navigation by Role

### ????? For QA Engineers

**Getting Started**
1. Read `QUICK_START.md` (5 min)
2. Read `README.md` - Setup section
3. Run `dotnet test` locally

**Writing Tests**
1. Review `CONTRIBUTING.md` - Test Structure
2. Look at examples in `Tests/Integration/InvoiceTests.cs`
3. Create new test class following pattern

**Debugging Issues**
1. Check `logs/` directory
2. Check `screenshots/` on failure
3. Review troubleshooting in `README.md`

### ?? For DevOps Engineers

**Pipeline Setup**
1. Read `CI-CD_SETUP_GUIDE.md` - your platform section
2. Copy template (Azure/GitHub/etc)
3. Follow step-by-step instructions

**Monitoring**
1. See monitoring section in `CI-CD_SETUP_GUIDE.md`
2. Configure notifications
3. Setup artifact retention

**Troubleshooting**
1. Check CI-CD setup guide - troubleshooting section
2. Review artifact logs
3. Check test output

### ????? For Project Managers

**Project Status**: `FINAL_STATUS.md`
- Requirements checklist
- Deliverables summary
- Quality metrics

**What Was Done**: `IMPLEMENTATION_SUMMARY.md`
- Before/after comparison
- Key improvements
- Timeline

**Documentation**: `README.md`
- Feature overview
- Test case descriptions
- Architecture summary

### ????? For Architects

**Architecture**: `CONTRIBUTING.md` - Architecture Patterns
- Page Object Model
- Service Layer
- Dependency Injection

**Best Practices**: `CONTRIBUTING.md`
- Design patterns
- Performance considerations
- CI/CD considerations

**Code Quality**: All `.cs` files
- XML documentation
- SOLID principles
- Error handling

---

## ?? Quick Reference Tables

### Configuration by Environment

| Setting | Development | Production/CI-CD |
|---------|-------------|------------------|
| Headless | false | true |
| Page Timeout | 60s | 30s |
| Element Timeout | 15s | 10s |
| Retries | 1 | 3 |
| Browser Args | None | Resource blocking |

### Command Quick Reference

| Task | Command |
|------|---------|
| Run all tests | `dotnet test` |
| Run one test | `dotnet test --filter="TestName"` |
| Build only | `dotnet build` |
| Clean build | `dotnet clean && dotnet build` |
| Local dev mode | `set ASPNETCORE_ENVIRONMENT=Development` |
| CI/CD mode | `set ASPNETCORE_ENVIRONMENT=Production` |
| Verbose output | `dotnet test --logger:"console;verbosity=detailed"` |
| Parallel tests | `dotnet test /p:ParallelizeAssembly=true` |

### Test Case Reference

| Test | File | Purpose |
|------|------|---------|
| Navigation | `InvoiceTests.cs` | Menu navigation |
| Summary Total | `InvoiceTests.cs` | Invoice sum validation |
| Specific Amount | `InvoiceTests.cs` | Single invoice verification |

---

## ?? Common Workflows

### Workflow 1: Run Tests Locally
```
1. Edit appsettings.Development.json (set URL)
2. set ASPNETCORE_ENVIRONMENT=Development
3. dotnet test
4. Review output / screenshots / logs
```

### Workflow 2: Add New Test
```
1. Create method in InvoiceTests.cs
2. Use pattern: [Test] public async Task Name_Scenario_Should...
3. Follow Arrange-Act-Assert
4. dotnet test --filter="NewTestName"
5. Commit with message: [TEST] Add new test case
```

### Workflow 3: Setup CI/CD Pipeline
```
1. Choose platform (Azure/GitHub/Jenkins/GitLab)
2. Open CI-CD_SETUP_GUIDE.md
3. Follow platform-specific section
4. Push code
5. Pipeline auto-triggers
6. Monitor results
```

### Workflow 4: Debug Failing Test
```
1. Run test locally: dotnet test --filter="FailingTest"
2. Check logs in logs/ directory
3. Check screenshot in screenshots/
4. Increase timeout if network issue
5. Add logging to understand flow
6. Re-run to verify fix
```

---

## ?? Help & Support

### Documentation Map

| Question | Document | Section |
|----------|----------|---------|
| How do I get started? | `QUICK_START.md` | 5-Minute Setup |
| How do I install? | `README.md` | Installation |
| How do I configure? | `README.md` | Configuration |
| How do I write tests? | `CONTRIBUTING.md` | Test Structure |
| How do I setup CI/CD? | `CI-CD_SETUP_GUIDE.md` | Platform section |
| What went wrong? | `README.md` | Troubleshooting |
| What was implemented? | `IMPLEMENTATION_SUMMARY.md` | Overview |
| What's the status? | `FINAL_STATUS.md` | Project Status |
| What's the code style? | `CONTRIBUTING.md` | Code Style |

### Getting Help

1. **First Check**: Docs related to your task
2. **Then Search**: README troubleshooting section
3. **Review**: Logs in `logs/` directory
4. **Examine**: Screenshots in `screenshots/`
5. **Ask**: Include logs and error details

---

## ? Key Takeaways

- ?? **Well-organized**: Layered architecture, clear separation of concerns
- ?? **Well-documented**: 5 comprehensive guides + inline code comments
- ?? **Production-ready**: CI/CD templates for 4 platforms
- ??? **Resilient**: Retry logic, fallback strategies, error handling
- ? **Fast**: Headless mode, resource blocking, optimized timeouts
- ?? **Extensible**: Easy to add new tests following established patterns

---

**Happy Testing! ??**

For questions, refer to the documentation in this folder.
For bugs/features, see CONTRIBUTING.md for submission guidelines.
