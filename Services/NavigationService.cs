using yuki_qa_automation_tests.PageObjects.Pages;
using yuki_qa_automation_tests.Utilities;
using System;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.Services
{
    /// <summary>
    /// Service for navigation operations
    /// </summary>
    public class NavigationService
    {
        private readonly MenuPage _menuPage;
        private readonly Logger _logger;

        public NavigationService(MenuPage menuPage, Logger logger)
        {
            _menuPage = menuPage ?? throw new ArgumentNullException(nameof(menuPage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Navigates through all menu pages and verifies menu is displayed
        /// </summary>
        public async Task<bool> NavigateToAllPagesAsync()
        {
            _logger.Info("Starting navigation through all pages");

            try
            {
                _logger.Info("Navigating to Home");
                await _menuPage.NavigateToHome();
                if (!await _menuPage.IsMenuDisplayed())
                {
                    _logger.Error("Menu not displayed on Home page");
                    return false;
                }

                _logger.Info("Navigating to Invoices");
                await _menuPage.NavigateToInvoices();
                if (!await _menuPage.IsMenuDisplayed())
                {
                    _logger.Error("Menu not displayed on Invoices page");
                    return false;
                }

                _logger.Info("Navigating to Customers");
                await _menuPage.NavigateToCustomers();
                if (!await _menuPage.IsMenuDisplayed())
                {
                    _logger.Error("Menu not displayed on Customers page");
                    return false;
                }

                _logger.Info("Navigating to Products");
                await _menuPage.NavigateToProducts();
                if (!await _menuPage.IsMenuDisplayed())
                {
                    _logger.Error("Menu not displayed on Products page");
                    return false;
                }

                _logger.Info("Navigation through all pages completed successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Navigation failed: {ex.Message}");
                return false;
            }
        }
    }
}
