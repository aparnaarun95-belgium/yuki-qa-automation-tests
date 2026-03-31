using Microsoft.Playwright;
using Reqnroll;
using System;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.Hooks
{
    [Binding]
    public class BrowserHooks
    {
        private readonly ScenarioContext _scenarioContext;
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IBrowserContext _context;
        private IPage _page;

        public BrowserHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            Console.WriteLine($"Starting scenario: {_scenarioContext.ScenarioInfo.Title}");

            try
            {
                _playwright = await Playwright.CreateAsync();

                _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = true,
                    SlowMo = 100
                });

                _context = await _browser.NewContextAsync(new BrowserNewContextOptions
                {
                    ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
                });

                _context.SetDefaultTimeout(30000);
                _context.SetDefaultNavigationTimeout(30000);

                _page = await _context.NewPageAsync();

                _scenarioContext["Page"] = _page;
                _scenarioContext["Browser"] = _browser;
                _scenarioContext["Context"] = _context;
                _scenarioContext["Playwright"] = _playwright;

                Console.WriteLine("Browser and page initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during browser initialization: {ex.Message}");
                throw;
            }
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            try
            {
                if (_page != null)
                {
                    await _page.CloseAsync();
                }

                if (_context != null)
                {
                    await _context.CloseAsync();
                }

                if (_browser != null)
                {
                    await _browser.CloseAsync();
                }

                if (_playwright != null)
                {
                    _playwright.Dispose();
                }

                Console.WriteLine("Browser and page closed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during browser cleanup: {ex.Message}");
            }
        }

        [AfterScenario(Order = 1)]
        public async Task AfterScenarioOnFailure()
        {
            if (_scenarioContext.TestError != null)
            {
                Console.WriteLine($"Scenario failed: {_scenarioContext.TestError.Message}");

                try
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                    string screenshotPath = $"screenshots/screenshot-{_scenarioContext.ScenarioInfo.Title}-{timestamp}.png";

                    if (_page != null)
                    {
                        await _page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
                        Console.WriteLine($"Screenshot saved to {screenshotPath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not take screenshot: {ex.Message}");
                }
            }
        }
    }
}
