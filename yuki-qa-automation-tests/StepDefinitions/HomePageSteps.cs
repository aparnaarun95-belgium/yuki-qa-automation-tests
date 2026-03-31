using Microsoft.Playwright;
using Reqnroll;
using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Playwright;
using yuki_qa_automation_tests.Pages;

namespace yuki_qa_automation_tests.StepDefinitions
{
    [Binding]
    public class HomePageSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private IPage _page;
        private HomePage _homePage;

        public HomePageSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        }

        [Given(@"I navigate to the home page")]
        public async Task GivenINavigateToTheHomePage()
        {
            _page = (IPage)_scenarioContext["Page"];
            _homePage = new HomePage(_page, 30000);

            await _homePage.NavigateToAsync("http://localhost:5000");
            Console.WriteLine("Navigated to home page");
        }

        [When(@"I wait for the home page to load")]
        public async Task WhenIWaitForTheHomePageToLoad()
        {
            // Ensure _homePage is initialized
            if (_homePage == null)
            {
                _page = (IPage)_scenarioContext["Page"];
                _homePage = new HomePage(_page, 30000);
            }

            var isLoaded = await _homePage.IsHomePageLoadedAsync();
            if (!isLoaded)
            {
                throw new InvalidOperationException("Home page did not load within the expected time");
            }
            Console.WriteLine("Home page loaded successfully");
        }

        [Then(@"the home page should be displayed")]
        public async Task ThenTheHomePageShouldBeDisplayed()
        {
            // Ensure _homePage is initialized
            if (_homePage == null)
            {
                _page = (IPage)_scenarioContext["Page"];
                _homePage = new HomePage(_page, 30000);
            }

            var isLoaded = await _homePage.IsHomePageLoadedAsync();
            Assert.That(isLoaded, Is.True, "Home page is not displayed");
            Console.WriteLine("? Home page is displayed");
        }

        [Then(@"the welcome heading should be visible")]
        public async Task ThenTheWelcomeHeadingShouldBeVisible()
        {
            // Ensure _homePage is initialized
            if (_homePage == null)
            {
                _page = (IPage)_scenarioContext["Page"];
                _homePage = new HomePage(_page, 30000);
            }

            var heading = await _homePage.GetWelcomeHeadingAsync();
            Assert.That(heading, Is.Not.Null.And.Not.Empty, "Welcome heading is not visible or empty");
            Console.WriteLine($"? Welcome heading is visible: '{heading}'");
        }

        [Then(@"the home page title should be ""(.*)""")]
        public async Task ThenTheHomePageTitleShouldBe(string expectedTitle)
        {
            _page = (IPage)_scenarioContext["Page"];
            var pageTitle = await _page.TitleAsync();
            Assert.That(pageTitle, Is.EqualTo(expectedTitle).IgnoreCase, $"Expected title '{expectedTitle}' but got '{pageTitle}'");
            Console.WriteLine($"? Page title is '{expectedTitle}'");
        }
    }
}
