using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using yuki_qa_automation_tests.Configuration;
using yuki_qa_automation_tests.Utilities;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.Core
{
    /// <summary>
    /// Base test class with setup and teardown logic
    /// Implements best practices for automated testing including resilience patterns
    /// </summary>
    public abstract class BaseTest
    {
        protected IPage Page;
        protected IBrowser Browser;
        protected IPlaywright Playwright;
        protected Logger Logger;
        protected TestSettings TestSettings;
        protected DriverFactory DriverFactory;

        [SetUp]
        public async Task Setup()
        {
            Logger = new Logger(GetType().Name);
            Logger.Info("Test setup started");

            try
            {
                // Load configuration
                var configuration = LoadConfiguration();
                TestSettings = TestSettings.LoadFromConfiguration(configuration);

                Logger.Info($"Environment: {GetEnvironment()}, Browser: {TestSettings.BrowserConfig.BrowserType}, Headless: {TestSettings.BrowserConfig.Headless}");

                // Initialize browser with retry logic for CI/CD resilience
                await InitializeBrowserWithRetryAsync();

                Logger.Info("Test setup completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Test setup failed: {ex.Message}");
                await CleanupAsync();
                throw;
            }
        }

        /// <summary>
        /// Initializes browser with retry logic for transient failures
        /// </summary>
        private async Task InitializeBrowserWithRetryAsync()
        {
            int attempt = 0;
            Exception lastException = null;

            while (attempt < TestSettings.RetryAttempts)
            {
                try
                {
                    DriverFactory = new DriverFactory(TestSettings.BrowserConfig, Logger);
                    Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
                    Browser = await DriverFactory.CreateBrowserAsync();
                    Page = await DriverFactory.CreatePageAsync(Browser);
                    
                    Logger.Info($"Browser initialized successfully on attempt {attempt + 1}");
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    attempt++;
                    
                    if (attempt < TestSettings.RetryAttempts)
                    {
                        Logger.Warning($"Browser initialization failed (attempt {attempt}): {ex.Message}. Retrying in {TestSettings.RetryDelayMs}ms...");
                        await Task.Delay(TestSettings.RetryDelayMs);
                    }
                    else
                    {
                        Logger.Error($"Browser initialization failed after {TestSettings.RetryAttempts} attempts");
                    }
                }
            }

            throw lastException ?? new Exception("Browser initialization failed");
        }

        [TearDown]
        public async Task TearDown()
        {
            Logger.Info("Test teardown started");

            try
            {
                if (TestFailed())
                {
                    await CaptureScreenshot();
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error during screenshot capture: {ex.Message}");
            }

            await CleanupAsync();
            Logger.Info("Test teardown completed");
        }

        /// <summary>
        /// Cleans up browser and Playwright resources
        /// </summary>
        private async Task CleanupAsync()
        {
            try
            {
                if (Page != null)
                {
                    try
                    {
                        await Page.CloseAsync();
                        Logger.Info("Page closed");
                    }
                    catch (Exception ex)
                    {
                        Logger.Warning($"Error closing page: {ex.Message}");
                    }
                }

                if (Browser != null)
                {
                    try
                    {
                        await Browser.CloseAsync();
                        Logger.Info("Browser closed");
                    }
                    catch (Exception ex)
                    {
                        Logger.Warning($"Error closing browser: {ex.Message}");
                    }
                }

                Playwright?.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error($"Error during cleanup: {ex.Message}");
            }
        }

        /// <summary>
        /// Captures a screenshot on test failure for diagnostics
        /// </summary>
        protected async Task CaptureScreenshot()
        {
            if (Page == null)
            {
                Logger.Warning("Cannot capture screenshot: Page is null");
                return;
            }

            try
            {
                var screenshotPath = TestSettings.BrowserConfig.ScreenshotPath;
                Directory.CreateDirectory(screenshotPath);

                var fileName = $"{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
                var fullPath = Path.Combine(screenshotPath, fileName);

                await Page.ScreenshotAsync(new PageScreenshotOptions { Path = fullPath, FullPage = true });
                Logger.Info($"Screenshot saved: {fullPath}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to capture screenshot: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if the current test has failed
        /// </summary>
        protected bool TestFailed()
        {
            return TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed;
        }

        /// <summary>
        /// Loads configuration from appsettings.json with environment-specific overrides
        /// </summary>
        private IConfiguration LoadConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{GetEnvironment()}.json", optional: true)
                .AddEnvironmentVariables("YUKI_");

            return configBuilder.Build();
        }

        /// <summary>
        /// Gets the current environment for configuration selection
        /// Defaults to Development for local testing
        /// </summary>
        private string GetEnvironment()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        }
    }
}
