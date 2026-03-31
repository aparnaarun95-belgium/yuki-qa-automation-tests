using Microsoft.Playwright;
using yuki_qa_automation_tests.Utilities;
using System;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.PageObjects.Pages
{
    /// <summary>
    /// Base class for all page objects
    /// </summary>
    public abstract class BasePage
    {
        protected IPage Page { get; set; }
        protected string BaseUrl { get; set; }
        protected Logger Logger { get; set; }
        protected WaitHelper WaitHelper { get; set; }

        public BasePage(IPage page, string baseUrl, Logger logger)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
            BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            WaitHelper = new WaitHelper(page, logger);
        }

        /// <summary>
        /// Navigates to the specified URL
        /// </summary>
        public async Task Navigate(string url)
        {
            Logger.Info($"Navigating to: {url}");
            await Page.GotoAsync(url);
            await WaitHelper.WaitForPageLoadAsync();
            Logger.Info("Page loaded successfully");
        }

        /// <summary>
        /// Clicks an element using CSS selector
        /// </summary>
        protected async Task Click(string selector)
        {
            Logger.Info($"Clicking element: {selector}");
            await WaitHelper.WaitForElementAsync(selector);
            await Page.ClickAsync(selector);
            await WaitHelper.WaitForPageLoadAsync();
        }

        /// <summary>
        /// Gets text content of an element
        /// </summary>
        protected async Task<string> GetText(string selector)
        {
            Logger.Info($"Getting text from element: {selector}");
            await WaitHelper.WaitForElementAsync(selector);
            var text = await Page.TextContentAsync(selector);
            return text?.Trim();
        }

        /// <summary>
        /// Checks if an element is displayed
        /// </summary>
        protected async Task<bool> IsElementDisplayed(string selector)
        {
            try
            {
                var element = await Page.QuerySelectorAsync(selector);
                return element != null && await element.IsVisibleAsync();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Closes the page
        /// </summary>
        public async Task Close()
        {
            await Page.CloseAsync();
            Logger.Info("Page closed");
        }
    }
}
