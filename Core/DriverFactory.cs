using Microsoft.Playwright;
using yuki_qa_automation_tests.Configuration;
using yuki_qa_automation_tests.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.Core
{
    /// <summary>
    /// Factory for creating and managing Playwright browser instances
    /// Ensures proper initialization and cleanup for CI/CD environments
    /// </summary>
    public class DriverFactory
    {
        private readonly BrowserConfig _browserConfig;
        private readonly Logger _logger;

        public DriverFactory(BrowserConfig browserConfig, Logger logger)
        {
            _browserConfig = browserConfig ?? throw new ArgumentNullException(nameof(browserConfig));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates a new browser instance based on configuration
        /// Optimized for CI/CD pipeline execution
        /// </summary>
        public async Task<IBrowser> CreateBrowserAsync()
        {
            _logger.Info($"Creating {_browserConfig.BrowserType} browser instance");

            try
            {
                var playwright = await Playwright.CreateAsync();
                
                var launchOptions = new BrowserTypeLaunchOptions
                {
                    Headless = _browserConfig.Headless,
                    Timeout = _browserConfig.TimeoutMs,
                    SlowMo = _browserConfig.SlowMo
                };

                // Add browser arguments for CI/CD environments
                if (_browserConfig.Args != null && _browserConfig.Args.Length > 0)
                {
                    var argsList = new List<string>(_browserConfig.Args);
                    launchOptions.Args = argsList;
                    _logger.Info($"Browser launch args: {string.Join(", ", argsList)}");
                }

                IBrowser browser = _browserConfig.BrowserType.ToLower() switch
                {
                    "chromium" => await playwright.Chromium.LaunchAsync(launchOptions),
                    "firefox" => await playwright.Firefox.LaunchAsync(launchOptions),
                    "webkit" => await playwright.Webkit.LaunchAsync(launchOptions),
                    _ => throw new ArgumentException($"Unknown browser type: {_browserConfig.BrowserType}")
                };

                _logger.Info($"Browser instance created successfully");
                return browser;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to create browser instance: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates a new page with context
        /// Configured with performance-optimized settings for CI/CD
        /// </summary>
        public async Task<IPage> CreatePageAsync(IBrowser browser)
        {
            _logger.Info("Creating new page context");

            try
            {
                var contextOptions = new BrowserNewContextOptions
                {
                    RecordVideoDir = _browserConfig.RecordVideo ? _browserConfig.VideoPath : null,
                    Locale = "en-US",
                    TimezoneId = "America/New_York"
                };

                var context = await browser.NewContextAsync(contextOptions);

                var page = await context.NewPageAsync();
                page.SetDefaultTimeout(_browserConfig.TimeoutMs);
                page.SetDefaultNavigationTimeout(_browserConfig.NavigationTimeoutMs);

                // Optimize for CI/CD performance
                await page.RouteAsync("**/*.{png,jpg,jpeg,gif,svg,webp,woff,woff2,ttf,otf,eot}", route => route.AbortAsync());

                _logger.Info("Page context created successfully with navigation timeout: " + _browserConfig.NavigationTimeoutMs);
                return page;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to create page context: {ex.Message}");
                throw;
            }
        }
    }
}
