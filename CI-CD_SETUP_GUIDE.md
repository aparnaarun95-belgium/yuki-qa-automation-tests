# CI/CD Setup Guide

This document provides step-by-step instructions for setting up the Yuki QA Automation Tests in various CI/CD environments.

## Table of Contents
- [Azure Pipelines](#azure-pipelines)
- [GitHub Actions](#github-actions)
- [Jenkins](#jenkins)
- [GitLab CI](#gitlab-ci)
- [General Troubleshooting](#general-troubleshooting)

## Azure Pipelines

### Prerequisites
- Azure DevOps project
- Azure Pipelines enabled
- Access to project settings

### Setup Steps

1. **Copy pipeline file to repository root**:
   ```bash
   cp azure-pipelines.yml ./
   git add azure-pipelines.yml
   git commit -m "[DEVOPS] Add Azure Pipelines configuration"
   git push
   ```

2. **Create Azure Pipeline**:
   - Go to Azure DevOps > Pipelines > Create Pipeline
   - Select "Existing Azure Pipelines YAML file"
   - Select `azure-pipelines.yml`
   - Click "Save and run"

3. **Configure Variables** (optional):
   - Go to Pipeline settings
   - Add variables:
     ```
     ASPNETCORE_ENVIRONMENT: Production
     BUILD_CONFIGURATION: Release
     ```

4. **Set up artifact retention** (optional):
   - Pipeline settings > Artifact retention
   - Set retention to 30 days

### Scheduled Runs
The pipeline automatically runs on:
- Push to `main` or `develop` branches
- Pull requests to `main` or `develop`
- Can be manually triggered

### View Results
- **Test Results**: Pipelines > [Run] > Test Results
- **Code Coverage**: Pipelines > [Run] > Code Coverage
- **Artifacts**: Pipelines > [Run] > Published artifacts

---

## GitHub Actions

### Prerequisites
- GitHub repository
- GitHub Actions enabled (default for public repos)

### Setup Steps

1. **Workflows are auto-detected**:
   - GitHub Actions automatically discovers workflows in `.github/workflows/`
   - File `.github/workflows/playwright-tests.yml` is already in place

2. **View workflow runs**:
   - Go to repository > Actions tab
   - Select "Playwright Tests" workflow
   - View run history and results

3. **Configure workflow triggers** (optional):
   - Edit `.github/workflows/playwright-tests.yml`
   - Modify `on:` section for different triggers

### Test Reports
- **Console Output**: Actions > [Run] > Playwright Tests job
- **Artifacts**: 
  - Screenshots (on failure)
  - Logs
  - Test results

### Branch Protection Rules
1. Go to Settings > Branches > Add rule
2. Require "Playwright Tests" to pass before merge:
   ```
   Status check: Playwright Tests
   Required: ?
   ```

---

## Jenkins

### Prerequisites
- Jenkins server running
- Git plugin installed
- .NET Core plugin installed
- MSBuild plugin installed

### Setup Steps

1. **Create New Job**:
   - Jenkins Dashboard > New Item
   - Name: `yuki-qa-automation-tests`
   - Select "Pipeline"
   - Click OK

2. **Configure Pipeline**:
   - Pipeline section > Definition: `Pipeline script from SCM`
   - SCM: Git
   - Repository URL: `https://github.com/your-org/yuki-qa-automation-tests.git`
   - Branches to build: `*/main`

3. **Create Jenkinsfile** (if not using pipeline from SCM):
   ```groovy
   pipeline {
       agent any
       
       environment {
           DOTNET_VERSION = '3.1'
           ASPNETCORE_ENVIRONMENT = 'Production'
       }
       
       stages {
           stage('Build') {
               steps {
                   sh 'dotnet restore'
                   sh 'dotnet build --configuration Release'
               }
           }
           
           stage('Test') {
               steps {
                   sh '''
                       pwsh bin/Release/netcoreapp3.1/playwright.ps1 install
                       dotnet test --configuration Release --logger "trx;LogFileName=test-results.trx"
                   '''
               }
           }
       }
       
       post {
           always {
               junit 'TestResults/*.trx'
               archiveArtifacts artifacts: 'screenshots/**/*.png,logs/**/*.log'
           }
       }
   }
   ```

4. **Configure Jenkins**:
   - Jenkins > Manage Jenkins > Configure System
   - Add .NET Core SDK: 3.1.x
   - Save

5. **Build Now**:
   - Click "Build Now" to trigger first run

---

## GitLab CI

### Prerequisites
- GitLab project
- GitLab Runner configured
- Docker or shell executor available

### Setup Steps

1. **Create `.gitlab-ci.yml`**:
   ```yaml
   image: mcr.microsoft.com/dotnet/sdk:3.1

   stages:
     - build
     - test

   before_script:
     - apt-get update && apt-get install -y powershell

   build:
     stage: build
     script:
       - dotnet restore
       - dotnet build --configuration Release
     artifacts:
       paths:
         - bin/Release/

   test:
     stage: test
     script:
       - pwsh bin/Release/netcoreapp3.1/playwright.ps1 install
       - pwsh bin/Release/netcoreapp3.1/playwright.ps1 install-deps
       - dotnet test --configuration Release --logger "trx;LogFileName=test-results.trx"
     artifacts:
       when: always
       paths:
         - screenshots/
         - logs/
         - TestResults/
       reports:
         junit: TestResults/test-results.trx
     env:
       ASPNETCORE_ENVIRONMENT: Production

   artifacts:
     when: always
     paths:
       - screenshots/
       - logs/
   ```

2. **Push to GitLab**:
   ```bash
   git add .gitlab-ci.yml
   git commit -m "[DEVOPS] Add GitLab CI configuration"
   git push
   ```

3. **View Pipeline**:
   - Go to project > CI/CD > Pipelines
   - Monitor pipeline execution

---

## General Troubleshooting

### Common Issues

#### 1. Playwright Browsers Not Found
```bash
# In your pipeline, ensure browsers are installed:
pwsh bin/Release/netcoreapp3.1/playwright.ps1 install
pwsh bin/Release/netcoreapp3.1/playwright.ps1 install-deps
```

#### 2. Tests Pass Locally but Fail in CI/CD

**Check configurations**:
- Verify `appsettings.json` has CI/CD optimized timeouts
- Set `ASPNETCORE_ENVIRONMENT=Production`
- Check logs in CI/CD artifacts

**Increase timeouts**:
```json
{
  "TestSettings": {
    "PageLoadTimeout": 60000,
    "NavigationTimeout": 60000,
    "ElementWaitTimeout": 15000,
    "RetryAttempts": 3
  }
}
```

#### 3. Resource Limits in Docker

Add to Playwright arguments:
```json
"Args": [
  "--disable-dev-shm-usage",
  "--disable-gpu",
  "--no-sandbox",
  "--disable-setuid-sandbox"
]
```

#### 4. Network Connectivity Issues

**Enable verbose logging**:
```bash
set CI=true
dotnet test --logger:"console;verbosity=detailed"
```

**Troubleshoot specific URL**:
```csharp
Logger.Debug($"Connecting to: {TestSettings.BaseUrl}");
Logger.Debug($"Network timeout: {TestSettings.TestSettings.NavigationTimeout}ms");
```

### Debugging CI/CD Issues

1. **Enable Debug Logging**:
   ```bash
   # Azure Pipelines
   System.Debug: true
   
   # GitHub Actions
   secrets.ACTIONS_STEP_DEBUG: true
   
   # Jenkins
   Enable verbose logging in job configuration
   ```

2. **Capture Environment Details**:
   ```csharp
   Logger.Info($"OS: {RuntimeInformation.OSDescription}");
   Logger.Info($".NET Version: {RuntimeInformation.FrameworkDescription}");
   Logger.Info($"Browser: {TestSettings.BrowserConfig.BrowserType}");
   Logger.Info($"Headless: {TestSettings.BrowserConfig.Headless}");
   ```

3. **Review Artifacts**:
   - Screenshots from failed tests
   - Log files with timestamps
   - Test output/console logs

### Performance Optimization Tips

1. **Parallel Test Execution**:
   ```bash
   dotnet test /p:ParallelizeAssembly=true
   ```

2. **Cache Dependencies**:
   - GitHub Actions: Automatic via actions/cache
   - Azure Pipelines: Configure cache task
   - GitLab CI: Configure cache policy

3. **Use Lightweight Images**:
   ```
   # Good for CI/CD
   mcr.microsoft.com/dotnet/sdk:3.1-alpine
   
   # More resources needed
   mcr.microsoft.com/dotnet/sdk:3.1
   ```

4. **Increase Timeouts Only When Needed**:
   ```json
   // Environment-specific configuration
   {
     "PageLoadTimeout": 45000,      // Production
     "ElementWaitTimeout": 12000,
     "RetryAttempts": 3             // More resilience
   }
   ```

---

## Environment Variables

### Required
- `ASPNETCORE_ENVIRONMENT`: Set to `Production` for CI/CD

### Optional
- `CI`: Set to `true` to enable debug logging
- `TF_BUILD`: Automatically set by Azure Pipelines
- `GITHUB_ACTIONS`: Automatically set by GitHub Actions

### Example CI/CD Configuration
```bash
ASPNETCORE_ENVIRONMENT=Production
CI=true
BUILD_CONFIGURATION=Release
```

---

## Monitoring & Alerts

### Azure Pipelines
- Set up branch policies to require pipeline success
- Configure retention policy for test results
- Enable email notifications on failure

### GitHub Actions
- Use branch protection rules
- Configure auto-deployment on success
- Set up Slack/email notifications via third-party actions

### Jenkins
- Configure email notifications
- Set up webhook triggers
- Enable pipeline metrics dashboard

---

## Best Practices for CI/CD

1. ? **Run tests on every push** to main/develop
2. ? **Schedule nightly runs** for stability testing
3. ? **Capture artifacts** (screenshots, logs) on failure
4. ? **Set appropriate timeouts** for your network
5. ? **Use headless mode** in CI/CD
6. ? **Monitor test duration** and optimize slow tests
7. ? **Document configuration** changes
8. ? **Version your pipeline** with code

---

For additional support, refer to:
- [Azure Pipelines Documentation](https://docs.microsoft.com/en-us/azure/devops/pipelines)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Jenkins Pipeline Documentation](https://www.jenkins.io/doc/book/pipeline/)
- [GitLab CI Documentation](https://docs.gitlab.com/ee/ci/)
