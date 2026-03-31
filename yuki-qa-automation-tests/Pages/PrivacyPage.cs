using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace yuki_qa_automation_tests.Pages
{
    public class PrivacyPage : BasePage
    {
        // Selectors for privacy page elements
        private const string PageHeading = "h1";
        private const string NavMenu = "nav";
        private const string HomeLink = "#nav-item-link-home";
        private const string InvoicesLink = "#nav-item-link-invoices";

        public PrivacyPage(IPage page, int defaultTimeout = 10000)
            : base(page, defaultTimeout)
        {
        }

        public async Task<bool> IsPrivacyPageLoadedAsync()
        {
            return await IsElementVisibleAsync(PageHeading);
        }

        public async Task<bool> IsNavigationMenuVisibleAsync()
        {
            return await IsElementVisibleAsync(NavMenu);
        }

        public async Task ClickHomeAsync()
        {
            await ClickAsync(HomeLink);
        }

        public async Task ClickInvoicesLinkAsync()
        {
            await ClickAsync(InvoicesLink);
        }
    }
}
