using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace yuki_qa_automation_tests.Base
{
    public abstract class BaseTest
    {
        protected IPage Page { get; private set; }
        protected IPlaywright Playwright { get; private set; }
        protected IBrowser Browser { get; private set; }
        protected IBrowserContext Context { get; private set; }

        protected string BaseUrl { get; set; } = "https://example.com";
        protected int DefaultTimeout { get; set; } = 30000;
        protected bool Headless { get; set; } = true;

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            TestContext.WriteLine("Global test setup started");
        }

        [OneTimeTearDown]
        public async Task GlobalTeardown()
        {
            TestContext.WriteLine("Global test teardown started");
        }

        [SetUp]
        public async Task InitializeAsync()
        {
            try
            {
                Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
                
                Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = Headless
                });

                Context = await Browser.NewContextAsync(new BrowserNewContextOptions
                {
                    ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
                });

                Context.SetDefaultTimeout(DefaultTimeout);
                Context.SetDefaultNavigationTimeout(DefaultTimeout);

                Page = await Context.NewPageAsync();
                Page.SetDefaultTimeout(DefaultTimeout);
                Page.SetDefaultNavigationTimeout(DefaultTimeout);

                TestContext.WriteLine($"Test environment initialized for {TestContext.CurrentContext.Test.Name}");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Error during test setup: {ex.Message}");
                throw;
            }
        }

        [TearDown]
        public async Task DisposeAsync()
        {
            try
            {
                // Check if test failed and capture screenshot
                if (TestContext.CurrentContext?.Result != null && 
                    TestContext.CurrentContext.Result.Outcome.Status != NUnit.Framework.Interfaces.TestStatus.Passed)
                {
                    await CaptureFailureScreenshot();
                }

                if (Page != null)
                {
                    await Page.CloseAsync();
                }

                if (Context != null)
                {
                    await Context.CloseAsync();
                }

                if (Browser != null)
                {
                    await Browser.CloseAsync();
                }

                Playwright?.Dispose();

                TestContext.WriteLine($"Test environment cleaned up for {TestContext.CurrentContext.Test.Name}");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Error during test cleanup: {ex.Message}");
            }
        }

        protected async Task NavigateToAsync(string url)
        {
            try
            {
                await Page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
                TestContext.WriteLine($"Navigated to: {url}");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Navigation error: {ex.Message}");
                throw;
            }
        }

        protected async Task<string> CaptureFailureScreenshot()
        {
            try
            {
                var testName = TestContext.CurrentContext.Test.Name;
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var screenshotDir = "Screenshots";
                
                if (!System.IO.Directory.Exists(screenshotDir))
                    System.IO.Directory.CreateDirectory(screenshotDir);

                var filepath = System.IO.Path.Combine(screenshotDir, $"{testName}_{timestamp}_FAILURE.png");
                await Page.ScreenshotAsync(new PageScreenshotOptions { Path = filepath, FullPage = true });
                
                TestContext.WriteLine($"Failure screenshot saved: {filepath}");
                return filepath;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Failed to capture screenshot: {ex.Message}");
                return null;
            }
        }

        protected async Task WaitForNavigationAsync(Func<Task> action)
        {
            var navigationTask = Page.WaitForNavigationAsync();
            await action();
            await navigationTask;
        }
    }
}
