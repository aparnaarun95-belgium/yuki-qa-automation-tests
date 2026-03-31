using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace yuki_qa_automation_tests.Pages
{
    /// <summary>
    /// Privacy Page Object Model.
    /// Inherits navigation functionality from NavigablePage to avoid code duplication.
    /// </summary>
    public class PrivacyPage : NavigablePage
    {
        // Selectors for privacy page specific elements
        private const string PageHeading = "h1";

        public PrivacyPage(IPage page, int defaultTimeout = 10000)
            : base(page, defaultTimeout)
        {
        }

        /// <summary>
        /// Checks if the privacy page is loaded.
        /// </summary>
        public async Task<bool> IsPrivacyPageLoadedAsync()
        {
            return await IsElementVisibleAsync(PageHeading);
        }

        /// <summary>
        /// Implements the abstract method from NavigablePage.
        /// </summary>
        public override async Task<bool> IsPageLoadedAsync()
        {
            return await IsPrivacyPageLoadedAsync();
        }
    }
}
