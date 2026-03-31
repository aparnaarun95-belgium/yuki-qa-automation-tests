using Reqnroll;
using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Playwright;
using yuki_qa_automation_tests.Pages;

namespace yuki_qa_automation_tests.StepDefinitions
{
    /// <summary>
    /// Navigation-specific step definitions for Reqnroll BDD tests.
    /// Reusable steps for navigating between pages using menu.
    /// This class demonstrates code reusability - same navigation steps work for all pages.
    /// </summary>
    [Binding]
    public class NavigationSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private IPage _page;
        private NavigablePage _currentPage;

        public NavigationSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        }

        /// <summary>
        /// Navigates to home page using the navigation menu.
        /// Reusable from any page that has navigation menu.
        /// </summary>
        [When(@"I navigate to home page using menu")]
        public async Task WhenINavigateToHomePageUsingMenu()
        {
            _page = (IPage)_scenarioContext["Page"];
            var homePage = new HomePage(_page, 30000);

            await homePage.NavigateToHomeAsync();
            _scenarioContext["CurrentPage"] = homePage;

            Console.WriteLine("Navigated to home page using menu");
        }

        /// <summary>
        /// Navigates to invoices page using the navigation menu.
        /// Reusable from any page that has navigation menu.
        /// </summary>
        [When(@"I navigate to invoices page using menu")]
        public async Task WhenINavigateToInvoicesPageUsingMenu()
        {
            _page = (IPage)_scenarioContext["Page"];
            var invoicesPage = new InvoicesPage(_page, 30000);

            await invoicesPage.NavigateToInvoicesAsync();
            _scenarioContext["CurrentPage"] = invoicesPage;

            Console.WriteLine("Navigated to invoices page using menu");
        }

        /// <summary>
        /// Navigates to privacy page using the navigation menu.
        /// Reusable from any page that has navigation menu.
        /// </summary>
        [When(@"I navigate to privacy page using menu")]
        public async Task WhenINavigateToPrivacyPageUsingMenu()
        {
            _page = (IPage)_scenarioContext["Page"];
            var privacyPage = new PrivacyPage(_page, 30000);

            await privacyPage.NavigateToPrivacyAsync();
            _scenarioContext["CurrentPage"] = privacyPage;

            Console.WriteLine("Navigated to privacy page using menu");
        }

        /// <summary>
        /// Verifies that the navigation menu is visible on the current page.
        /// This is a generic check that works on any page with navigation.
        /// </summary>
        [Then(@"the navigation menu should be visible")]
        public async Task ThenTheNavigationMenuShouldBeVisible()
        {
            _page = (IPage)_scenarioContext["Page"];
            var homePage = new HomePage(_page, 30000);

            var isVisible = await homePage.IsNavigationMenuVisibleAsync();
            Assert.That(isVisible, Is.True, "Navigation menu is not visible");

            Console.WriteLine("? Navigation menu is visible");
        }

        /// <summary>
        /// Verifies that the current page is loaded.
        /// Uses polymorphism to verify any page that implements IsPageLoadedAsync.
        /// </summary>
        [Then(@"the current page should be loaded")]
        public async Task ThenTheCurrentPageShouldBeLoaded()
        {
            _currentPage = (NavigablePage)_scenarioContext.Get<object>("CurrentPage");

            if (_currentPage == null)
            {
                throw new InvalidOperationException("Current page not set in context");
            }

            var isLoaded = await _currentPage.IsPageLoadedAsync();
            Assert.That(isLoaded, Is.True, "Current page is not loaded");

            Console.WriteLine("? Current page is loaded");
        }
    }
}
