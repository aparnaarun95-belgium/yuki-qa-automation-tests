using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace yuki_qa_automation_tests.Pages
{
    /// <summary>
    /// Base navigation class that provides common navigation functionality across all pages.
    /// This class encapsulates menu navigation logic to avoid code duplication.
    /// </summary>
    public abstract class NavigablePage : BasePage
    {
        // Common navigation selectors (shared across all pages)
        protected const string NavMenu = "nav";
        protected const string HomeLink = "#nav-item-link-home";
        protected const string InvoicesLink = "#nav-item-link-invoices";
        protected const string PrivacyLink = "#nav-item-link-privacy";

        protected NavigablePage(IPage page, int defaultTimeout = 10000)
            : base(page, defaultTimeout)
        {
        }

        /// <summary>
        /// Verifies that the navigation menu is visible on the page.
        /// This is a reusable method that can be called from any page that inherits from this class.
        /// </summary>
        public virtual async Task<bool> IsNavigationMenuVisibleAsync()
        {
            return await IsElementVisibleAsync(NavMenu);
        }

        /// <summary>
        /// Navigates to home page using the navigation menu.
        /// Reusable across all pages.
        /// </summary>
        public virtual async Task NavigateToHomeAsync()
        {
            await ClickAsync(HomeLink);
            await WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        /// <summary>
        /// Navigates to invoices page using the navigation menu.
        /// Reusable across all pages.
        /// </summary>
        public virtual async Task NavigateToInvoicesAsync()
        {
            await ClickAsync(InvoicesLink);
            await WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        /// <summary>
        /// Navigates to privacy page using the navigation menu.
        /// Reusable across all pages.
        /// </summary>
        public virtual async Task NavigateToPrivacyAsync()
        {
            await ClickAsync(PrivacyLink);
            await WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        /// <summary>
        /// Generic method to verify if a page is loaded.
        /// Must be implemented by derived classes.
        /// </summary>
        public abstract Task<bool> IsPageLoadedAsync();
    }
}
