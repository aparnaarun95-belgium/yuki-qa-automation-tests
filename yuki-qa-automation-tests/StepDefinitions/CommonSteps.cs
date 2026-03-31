using Microsoft.Playwright;
using Reqnroll;
using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace yuki_qa_automation_tests.StepDefinitions
{
    [Binding]
    public class CommonSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private IPage _page;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        }

        [Given(@"I navigate to ""(.*)""")]
        public async Task GivenINavigateTo(string url)
        {
            _page = (IPage)_scenarioContext["Page"];
            await _page.GotoAsync(url);
            Console.WriteLine($"Navigated to {url}");
        }

        [When(@"I wait for (\d+) seconds")]
        public async Task WhenIWaitForSeconds(int seconds)
        {
            await Task.Delay(seconds * 1000);
            Console.WriteLine($"Waited for {seconds} seconds");
        }

        [When(@"I click on element with selector ""(.*)""")]
        public async Task WhenIClickOnElement(string selector)
        {
            _page = (IPage)_scenarioContext["Page"];
            await _page.ClickAsync(selector);
            Console.WriteLine($"Clicked on element: {selector}");
        }

        [When(@"I fill ""(.*)"" with ""(.*)""")]
        public async Task WhenIFillInput(string selector, string value)
        {
            _page = (IPage)_scenarioContext["Page"];
            await _page.FillAsync(selector, value);
            Console.WriteLine($"Filled {selector} with '{value}'");
        }

        [Then(@"I should see text ""(.*)""")]
        public async Task ThenIShouldSeeText(string text)
        {
            _page = (IPage)_scenarioContext["Page"];
            var content = await _page.ContentAsync();

            Assert.That(content, Does.Contain(text), $"Text '{text}' not found on page");
            Console.WriteLine($"? Text '{text}' is visible on page");
        }

        [Then(@"the page title should be ""(.*)""")]
        public async Task ThenThePageTitleShouldBe(string expectedTitle)
        {
            _page = (IPage)_scenarioContext["Page"];
            var pageTitle = await _page.TitleAsync();

            Assert.That(pageTitle, Is.EqualTo(expectedTitle).IgnoreCase, $"Expected title '{expectedTitle}' but got '{pageTitle}'");
            Console.WriteLine($"? Page title is '{expectedTitle}'");
        }

        [Then(@"the URL should be ""(.*)""")]
        public void ThenTheURLShouldBe(string expectedUrl)
        {
            _page = (IPage)_scenarioContext["Page"];
            var currentUrl = _page.Url;

            Assert.That(currentUrl, Is.EqualTo(expectedUrl).IgnoreCase, $"Expected URL '{expectedUrl}' but got '{currentUrl}'");
            Console.WriteLine($"? URL is '{expectedUrl}'");
        }

        [Then(@"the URL should contain ""(.*)""")]
        public void ThenTheURLShouldContain(string urlPart)
        {
            _page = (IPage)_scenarioContext["Page"];
            var currentUrl = _page.Url;

            Assert.That(currentUrl, Does.Contain(urlPart), $"URL '{currentUrl}' does not contain '{urlPart}'");
            Console.WriteLine($"? URL contains '{urlPart}'");
        }

        [Then(@"element with selector ""(.*)"" should be visible")]
        public async Task ThenElementShouldBeVisible(string selector)
        {
            _page = (IPage)_scenarioContext["Page"];
            var isVisible = await _page.IsVisibleAsync(selector);

            Assert.That(isVisible, Is.True, $"Element '{selector}' is not visible");
            Console.WriteLine($"? Element '{selector}' is visible");
        }

        [Then(@"element with selector ""(.*)"" should not be visible")]
        public async Task ThenElementShouldNotBeVisible(string selector)
        {
            _page = (IPage)_scenarioContext["Page"];
            var isVisible = await _page.IsVisibleAsync(selector);

            Assert.That(isVisible, Is.False, $"Element '{selector}' should not be visible");
            Console.WriteLine($"? Element '{selector}' is not visible");
        }
    }
}
