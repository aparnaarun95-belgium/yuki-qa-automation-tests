using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.Pages
{
    public abstract class BasePage
    {
        protected readonly IPage Page;
        protected readonly int DefaultTimeout;

        protected BasePage(IPage page, int defaultTimeout = 10000)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
            DefaultTimeout = defaultTimeout;
        }

        public string GetCurrentUrl() => Page.Url;

        public async Task<string> GetPageTitleAsync() => await Page.TitleAsync();

        public async Task<string> GetPageContentAsync() => await Page.ContentAsync();

        protected async Task<ILocator> FindElementAsync(string selector)
        {
            var locator = Page.Locator(selector);
            await locator.WaitForAsync(new LocatorWaitForOptions { Timeout = DefaultTimeout });
            return locator;
        }

        protected async Task<ILocator> FindElementVisibleAsync(string selector)
        {
            var locator = Page.Locator(selector);
            await locator.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = DefaultTimeout
            });
            return locator;
        }

        protected async Task ClickAsync(string selector)
        {
            var element = await FindElementVisibleAsync(selector);
            await element.ClickAsync();
        }

        protected async Task ClickAsync(ILocator locator)
        {
            await locator.ClickAsync();
        }

        protected async Task FillAsync(string selector, string text)
        {
            var element = await FindElementVisibleAsync(selector);
            await element.FillAsync(text);
        }

        protected async Task FillAsync(ILocator locator, string text)
        {
            await locator.FillAsync(text);
        }

        protected async Task<string> GetTextAsync(string selector)
        {
            var element = await FindElementAsync(selector);
            return await element.TextContentAsync();
        }

        protected async Task<string> GetTextAsync(ILocator locator)
        {
            return await locator.TextContentAsync();
        }

        protected async Task<string> GetAttributeAsync(string selector, string attributeName)
        {
            var element = await FindElementAsync(selector);
            return await element.GetAttributeAsync(attributeName);
        }

        protected async Task<bool> IsElementVisibleAsync(string selector)
        {
            try
            {
                await FindElementVisibleAsync(selector);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected async Task<bool> IsElementPresentAsync(string selector)
        {
            try
            {
                await FindElementAsync(selector);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected async Task HoverAsync(string selector)
        {
            var element = await FindElementAsync(selector);
            await element.HoverAsync();
        }

        protected async Task SelectOptionAsync(string selector, string value)
        {
            var element = await FindElementAsync(selector);
            await element.SelectOptionAsync(value);
        }

        protected async Task<bool> WaitForUrlChangeAsync(string expectedUrl, int timeoutMs = 10000)
        {
            try
            {
                await Page.WaitForURLAsync(expectedUrl, new PageWaitForURLOptions { Timeout = timeoutMs });
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected async Task WaitForLoadStateAsync(LoadState state = LoadState.NetworkIdle)
        {
            await Page.WaitForLoadStateAsync(state);
        }
    }
}
