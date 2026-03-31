using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace yuki_qa_automation_tests.Pages
{
    public class HomePage : NavigablePage
    {
        // Selectors for home page specific elements only
        private const string WelcomeHeading = "h1.display-4";

        public HomePage(IPage page, int defaultTimeout = 10000)
            : base(page, defaultTimeout)
        {
        }

        /// <summary>
        /// Checks if the home page is loaded by verifying the welcome heading is visible.
        /// </summary>
        public async Task<bool> IsHomePageLoadedAsync()
        {
            // Check if welcome heading is visible
            return await IsElementVisibleAsync(WelcomeHeading);
        }

        /// <summary>
        /// Implements the abstract method from NavigablePage.
        /// </summary>
        public override async Task<bool> IsPageLoadedAsync()
        {
            return await IsHomePageLoadedAsync();
        }

        public async Task<string> GetWelcomeHeadingAsync()
        {
            return await GetTextAsync(WelcomeHeading);
        }

        /// <summary>
        /// Legacy method name for backward compatibility.
        /// Navigates to invoices page using the menu.
        /// </summary>
        public async Task ClickInvoicesLinkAsync()
        {
            await NavigateToInvoicesAsync();
        }

        /// <summary>
        /// Legacy method name for backward compatibility.
        /// Navigates to privacy page using the menu.
        /// </summary>
        public async Task ClickPrivacyLinkAsync()
        {
            await NavigateToPrivacyAsync();
        }

        /// <summary>
        /// Legacy method name for backward compatibility.
        /// Navigates to home page using the menu.
        /// </summary>
        public async Task ClickHomeAsync()
        {
            await NavigateToHomeAsync();
        }

        public async Task WaitForHomePageLoadAsync()
        {
            await WaitForLoadStateAsync(LoadState.NetworkIdle);
            await FindElementVisibleAsync(WelcomeHeading);
        }
    }
}
