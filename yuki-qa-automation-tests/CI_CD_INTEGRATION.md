# ?? CI/CD Integration Guide

## Overview

This test automation framework is **production-ready** for CI/CD pipelines. This guide shows how to integrate it with popular CI/CD platforms.

---

## ? CI/CD Readiness Checklist

- ? **Headless Mode** - Tests run without UI
- ? **No Manual Intervention** - Fully automated
- ? **Configurable Timeouts** - Handles network latency
- ? **Artifact Collection** - Screenshots on failure
- ? **Exit Codes** - Pass/fail indication
- ? **Parallel Execution** - Fast test runs
- ? **Logging** - Structured output
- ? **Environment Support** - Dev/Prod configs

---

## GitHub Actions

### Basic Workflow

Create `.github/workflows/tests.yml`:

```yaml
name: Automated Tests

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  test:
    runs-on: ubuntu-latest
    strategy:
      matrix:
        dotnet-version: ['8.0']

    steps:
    - uses: actions/checkout@v3

    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: ${{ matrix.dotnet-version }}

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --configuration Release

    - name: Install Playwright browsers
      run: |
        dotnet tool install --global Microsoft.Playwright.CLI
        playwright install

    - name: Run tests
      run: dotnet test --configuration Release --logger "console;verbosity=detailed" --results-directory TestResults

    - name: Upload test results
      if: always()
      uses: actions/upload-artifact@v3
      with:
        name: test-results
        path: TestResults/

    - name: Upload screenshots
      if: failure()
      uses: actions/upload-artifact@v3
      with:
        name: failure-screenshots
        path: Screenshots/

    - name: Test Report
      if: always()
      uses: dorny/test-reporter@v1
      with:
        name: Test Results
        path: TestResults/*.xml
        reporter: 'java-junit'
```

### Advanced Workflow with Multiple Environments

```yaml
name: Multi-Environment Tests

on:
  push:
    branches: [ main ]
  schedule:
    - cron: '0 2 * * *'  # Daily at 2 AM

jobs:
  test:
    runs-on: ${{ matrix.os }}
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest, macos-latest]
        browser: [chromium, firefox]

    steps:
    - uses: actions/checkout@v3

    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0'

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --configuration Release

    - name: Install Playwright
      run: |
        dotnet tool install --global Microsoft.Playwright.CLI
        playwright install ${{ matrix.browser }}

    - name: Run tests on ${{ matrix.browser }}
      run: dotnet test --configuration Release --filter "Category=Smoke" --logger "console"
      env:
        BROWSER_TYPE: ${{ matrix.browser }}

    - name: Upload artifacts
      if: failure()
      uses: actions/upload-artifact@v3
      with:
        name: screenshots-${{ matrix.os }}-${{ matrix.browser }}
        path: Screenshots/
```

---

## Azure Pipelines

### Basic Pipeline

Create `azure-pipelines.yml`:

```yaml
trigger:
  - main
  - develop

pr:
  - main
  - develop

pool:
  vmImage: 'ubuntu-latest'

variables:
  buildConfiguration: 'Release'
  dotnetVersion: '8.0'

stages:
- stage: Build
  displayName: Build and Test
  jobs:
  - job: BuildAndTest
    displayName: 'Build and Run Tests'
    steps:

    - task: UseDotNet@2
      displayName: 'Setup .NET'
      inputs:
        version: $(dotnetVersion)
        packageType: 'sdk'

    - task: DotNetCoreCLI@2
      displayName: 'Restore NuGet packages'
      inputs:
        command: 'restore'

    - task: DotNetCoreCLI@2
      displayName: 'Build solution'
      inputs:
        command: 'build'
        arguments: '--configuration $(buildConfiguration)'

    - script: |
        dotnet tool install --global Microsoft.Playwright.CLI
        playwright install
      displayName: 'Install Playwright browsers'

    - task: DotNetCoreCLI@2
      displayName: 'Run automated tests'
      inputs:
        command: 'test'
        arguments: '--configuration $(buildConfiguration) --logger trx --results-directory $(Agent.TempDirectory)/TestResults'

    - task: PublishTestResults@2
      displayName: 'Publish test results'
      condition: succeededOrFailed()
      inputs:
        testResultsFormat: 'VSTest'
        testResultsFiles: '$(Agent.TempDirectory)/TestResults/*.trx'
        mergeTestResults: true
        publishRunAttachments: true

    - task: PublishBuildArtifacts@1
      displayName: 'Publish failure screenshots'
      condition: failed()
      inputs:
        pathToPublish: '$(Build.SourcesDirectory)/Screenshots'
        artifactName: 'failure-screenshots'
```

---

## GitLab CI

Create `.gitlab-ci.yml`:

```yaml
stages:
  - build
  - test
  - report

variables:
  DOTNET_VERSION: "8.0"

build:
  stage: build
  image: mcr.microsoft.com/dotnet/sdk:8.0
  script:
    - dotnet restore
    - dotnet build --configuration Release
  artifacts:
    paths:
      - bin/Release/
    expire_in: 1 hour

test:
  stage: test
  image: mcr.microsoft.com/dotnet/sdk:8.0
  script:
    - apt-get update && apt-get install -y libdbus-1-3 libxkbcommon0
    - dotnet tool install --global Microsoft.Playwright.CLI
    - export PATH="$PATH:/root/.dotnet/tools"
    - playwright install
    - dotnet test --configuration Release --logger "console;verbosity=detailed" --results-directory TestResults
  artifacts:
    paths:
      - TestResults/
      - Screenshots/
    when: always
    expire_in: 30 days
  allow_failure: false

pages:
  stage: report
  dependencies:
    - test
  script:
    - mkdir -p public
    - cp -r Screenshots/* public/ || true
  artifacts:
    paths:
      - public
    expire_in: 30 days
  only:
    - main
```

---

## Jenkins

### Jenkinsfile (Declarative Pipeline)

```groovy
pipeline {
    agent any

    options {
        timeout(time: 30, unit: 'MINUTES')
        timestamps()
        buildDiscarder(logRotator(numToKeepStr: '10'))
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Setup') {
            steps {
                script {
                    sh '''
                        dotnet --version
                        dotnet tool install --global Microsoft.Playwright.CLI
                        playwright install
                    '''
                }
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet restore'
                sh 'dotnet build --configuration Release'
            }
        }

        stage('Test') {
            steps {
                sh 'dotnet test --configuration Release --logger "console;verbosity=detailed" --results-directory TestResults'
            }
        }

        stage('Report') {
            steps {
                junit 'TestResults/**/*.trx'
                archiveArtifacts artifacts: 'Screenshots/**/*.png', allowEmptyArchive: true
            }
        }
    }

    post {
        always {
            cleanWs()
        }
        failure {
            emailext(
                subject: "Test Failure - ${env.BUILD_TAG}",
                body: "Tests failed. See attached screenshots.",
                to: "${env.CHANGE_AUTHOR_EMAIL}",
                attachmentsPattern: 'Screenshots/**/*.png'
            )
        }
    }
}
```

---

## Environment Configuration for CI/CD

### Using Environment Variables

Create `Config/appsettings.ci.json`:

```json
{
  "PlaywrightSettings": {
    "BaseUrl": "https://staging.example.com",
    "BrowserType": "chromium",
    "Headless": true,
    "SlowMo": 0,
    "Timeout": 45000,
    "NavigationTimeout": 45000,
    "ScreenshotOnFailure": true,
    "VideoOnFailure": false,
    "TraceOnFailure": false
  },
  "RetryPolicy": {
    "MaxRetries": 3,
    "DelayMilliseconds": 2000
  },
  "Waits": {
    "DefaultWaitTimeMs": 10000,
    "ElementWaitTimeMs": 15000,
    "NavigationWaitTimeMs": 45000
  }
}
```

### Update .csproj for Configuration Selection

```xml
<ItemGroup>
  <None Update="Config/appsettings.json">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
  <None Update="Config/appsettings.Development.json">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
  <None Update="Config/appsettings.ci.json">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
</ItemGroup>
```

---

## Docker Integration

### Dockerfile for CI/CD

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Install dependencies for Playwright
RUN apt-get update && apt-get install -y \
    libdbus-1-3 \
    libxkbcommon0 \
    libx11-xcb1 \
    libxcb-shm0 \
    libxcomposite1 \
    libxdamage1 \
    libxext6 \
    libxfixes3 \
    libxrandr2 \
    libxrender1 \
    libxtst6 \
    libxss1 \
    fonts-liberation \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy project files
COPY . .

# Restore and build
RUN dotnet restore
RUN dotnet build --configuration Release

# Install Playwright
RUN dotnet tool install --global Microsoft.Playwright.CLI
RUN export PATH="$PATH:/root/.dotnet/tools" && playwright install

# Run tests
CMD ["dotnet", "test", "--configuration", "Release", "--logger", "console;verbosity=detailed"]
```

### Docker Compose for Local Testing

```yaml
version: '3.8'

services:
  tests:
    build: .
    environment:
      - PLAYWRIGHT_HEADLESS=true
      - BASE_URL=http://web:3000
    volumes:
      - ./Screenshots:/app/Screenshots
      - ./TestResults:/app/TestResults
    depends_on:
      - web

  web:
    image: nginx:latest
    ports:
      - "3000:80"
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf:ro
```

---

## Performance Optimization for CI/CD

### Parallel Test Execution

```bash
# In your CI/CD pipeline
dotnet test -- NUnit.NumberOfTestWorkers=4
```

### Test Filtering

```bash
# Run only smoke tests (faster)
dotnet test --filter "Category=Smoke"

# Run specific test class
dotnet test --filter "ClassName~HomePageTests"
```

### Build Cache Strategy

**GitHub Actions Example:**
```yaml
- uses: actions/cache@v3
  with:
    path: ~/.nuget/packages
    key: ${{ runner.os }}-nuget-${{ hashFiles('**/packages.lock.json') }}
```

---

## Monitoring & Reporting

### Test Results Dashboard

Use tools to visualize test results:

1. **GitHub Actions** - Built-in test summary
2. **Azure DevOps** - Test analytics
3. **Jenkins** - Test Result Trend
4. **GitLab** - Test Reports

### Artifact Collection

```bash
# Screenshots collected in Screenshots/ folder
# Create summary report:
find Screenshots -name "*.png" -exec echo {} \; > report.txt
```

### Email Notifications

Example Python script for sending report:

```python
import smtplib
import os
from pathlib import Path

screenshots = list(Path("Screenshots").glob("*.png"))
failures = len(screenshots)

email_body = f"""
Test Execution Report
====================
Total Failures: {failures}

Failed Tests:
{chr(10).join([f"- {s.name}" for s in screenshots])}
"""

# Send email...
```

---

## Security Considerations

### Secrets Management

#### GitHub Actions:
```yaml
- name: Run tests with secrets
  env:
    TEST_USERNAME: ${{ secrets.TEST_USERNAME }}
    TEST_PASSWORD: ${{ secrets.TEST_PASSWORD }}
  run: dotnet test
```

#### Azure Pipelines:
```yaml
- task: DotNetCoreCLI@2
  inputs:
    command: 'test'
  env:
    TEST_USERNAME: $(TestUsername)
    TEST_PASSWORD: $(TestPassword)
```

### Configuration Security

```csharp
// Sensitive data in environment variables, not config files
var baseUrl = Environment.GetEnvironmentVariable("TEST_BASE_URL") 
    ?? "https://staging.example.com";
```

---

## Troubleshooting CI/CD

### Common Issues

| Issue | Solution |
|-------|----------|
| **Browser not found** | `playwright install` before tests |
| **Connection timeout** | Increase timeout in CI config |
| **Permission denied** | Run with proper permissions |
| **Out of memory** | Reduce parallel workers or split tests |
| **Screenshot path issue** | Create Screenshots/ folder in setup |

### Debug Logging

Enable verbose output:
```yaml
run: dotnet test -v d  # detailed logging
```

### Keep Artifacts

```yaml
artifacts:
  expire_in: 30 days  # or 'never' for permanent
```

---

## Best Practices

? **Run tests on every commit/PR**
? **Set appropriate timeouts** for your environment
? **Collect failure screenshots** for debugging
? **Run smoke tests first** (quick feedback)
? **Run full suite nightly** (comprehensive testing)
? **Monitor test trends** over time
? **Parallelize when possible** (faster feedback)
? **Cache dependencies** (faster builds)
? **Report results clearly** (easy investigation)
? **Notify on failures** (quick response)

---

## Example: Complete GitHub Actions Workflow

```yaml
name: E2E Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v3

    - uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0'

    - uses: actions/cache@v3
      with:
        path: ~/.nuget/packages
        key: ${{ runner.os }}-nuget-${{ hashFiles('**/packages.lock.json') }}

    - run: dotnet restore

    - run: dotnet build --configuration Release

    - run: |
        dotnet tool install --global Microsoft.Playwright.CLI
        playwright install

    - run: dotnet test --configuration Release

    - uses: actions/upload-artifact@v3
      if: failure()
      with:
        name: screenshots
        path: Screenshots/

    - uses: actions/upload-artifact@v3
      with:
        name: test-results
        path: TestResults/
```

---

**Your test framework is ready for enterprise CI/CD!** ??
