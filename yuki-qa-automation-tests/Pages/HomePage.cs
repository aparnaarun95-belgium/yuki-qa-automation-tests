using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace yuki_qa_automation_tests.Pages
{
    public class HomePage : BasePage
    {
        // Selectors for existing elements only
        private const string WelcomeHeading = "h1.display-4";
        private const string NavMenu = "nav";
        private const string InvoicesLink = "#nav-item-link-invoices";
        private const string PrivacyLink = "#nav-item-link-privacy";
        private const string HomeLink = "#nav-item-link-home";

        public HomePage(IPage page, int defaultTimeout = 10000)
            : base(page, defaultTimeout)
        {
        }

        public async Task<bool> IsHomePageLoadedAsync()
        {
            // Check if welcome heading is visible
            return await IsElementVisibleAsync(WelcomeHeading);
        }

        public async Task<string> GetWelcomeHeadingAsync()
        {
            return await GetTextAsync(WelcomeHeading);
        }

        public async Task<bool> IsNavigationMenuVisibleAsync()
        {
            return await IsElementVisibleAsync(NavMenu);
        }

        public async Task ClickInvoicesLinkAsync()
        {
            await ClickAsync(InvoicesLink);
        }

        public async Task ClickPrivacyLinkAsync()
        {
            await ClickAsync(PrivacyLink);
        }

        public async Task ClickHomeAsync()
        {
            await ClickAsync(HomeLink);
        }

        public async Task WaitForHomePageLoadAsync()
        {
            await WaitForLoadStateAsync(LoadState.NetworkIdle);
            await FindElementVisibleAsync(WelcomeHeading);
        }
    }
}
