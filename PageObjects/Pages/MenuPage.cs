using Microsoft.Playwright;
using yuki_qa_automation_tests.Utilities;
using System;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.PageObjects.Pages
{
    /// <summary>
    /// Page object for menu navigation
    /// </summary>
    public class MenuPage : BasePage
    {
        private const string HomeLink = "a:has-text('Home')";
        private const string InvoicesLink = "a:has-text('Invoices')";
        private const string CustomersLink = "a:has-text('Customers')";
        private const string ProductsLink = "a:has-text('Products')";

        public MenuPage(IPage page, string baseUrl, Logger logger) : base(page, baseUrl, logger)
        {
        }

        /// <summary>
        /// Navigates to Home page via menu
        /// </summary>
        public async Task NavigateToHome()
        {
            Logger.Info("Navigating to Home page");
            await Click(HomeLink);
        }

        /// <summary>
        /// Navigates to Invoices page via menu
        /// </summary>
        public async Task NavigateToInvoices()
        {
            Logger.Info("Navigating to Invoices page");
            await Click(InvoicesLink);
        }

        /// <summary>
        /// Navigates to Customers page via menu
        /// </summary>
        public async Task NavigateToCustomers()
        {
            Logger.Info("Navigating to Customers page");
            await Click(CustomersLink);
        }

        /// <summary>
        /// Navigates to Products page via menu
        /// </summary>
        public async Task NavigateToProducts()
        {
            Logger.Info("Navigating to Products page");
            await Click(ProductsLink);
        }

        /// <summary>
        /// Checks if menu is displayed
        /// </summary>
        public async Task<bool> IsMenuDisplayed()
        {
            var homeVisible = await IsElementDisplayed(HomeLink);
            var invoicesVisible = await IsElementDisplayed(InvoicesLink);
            return homeVisible && invoicesVisible;
        }
    }
}
