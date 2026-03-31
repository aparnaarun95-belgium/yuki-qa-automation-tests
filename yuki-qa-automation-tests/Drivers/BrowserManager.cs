using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.Drivers
{
    public class BrowserManager : IAsyncDisposable
    {
        private IBrowser _browser;
        private IPlaywright _playwright;
        private readonly string _browserType;
        private readonly bool _headless;
        private readonly int _slowMo;

        public BrowserManager(string browserType = "chromium", bool headless = true, int slowMo = 0)
        {
            _browserType = browserType;
            _headless = headless;
            _slowMo = slowMo;
        }

        public async Task<IBrowser> LaunchBrowserAsync()
        {
            if (_browser != null)
                return _browser;

            _playwright = await Playwright.CreateAsync();

            _browser = _browserType.ToLower() switch
            {
                "firefox" => await _playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = _headless,
                    SlowMo = _slowMo
                }),
                "chromium" => await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = _headless,
                    SlowMo = _slowMo
                }),
                _ => await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = _headless,
                    SlowMo = _slowMo
                })
            };

            return _browser;
        }

        public async ValueTask DisposeAsync()
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
            }

            _playwright?.Dispose();
        }
    }
}
