using NUnit.Framework;
using System.Threading.Tasks;
using yuki_qa_automation_tests.Base;
using yuki_qa_automation_tests.Pages;

namespace yuki_qa_automation_tests.Tests
{
    [TestFixture]
    [Category("UI")]
    [Category("Smoke")]
    public class HomePageTests : BaseTest
    {
        private HomePage _homePage;

        [SetUp]
        public async Task TestSetup()
        {
            _homePage = new HomePage(Page, DefaultTimeout);
            BaseUrl = "http://localhost:5000";
        }

        [Test]
        [Description("Verify home page loads successfully")]
        public async Task TC001_VerifyHomePageLoads()
        {
            // Arrange
            var expectedUrl = BaseUrl;

            // Act
            await _homePage.NavigateToAsync(expectedUrl);
            var isLoaded = await _homePage.IsHomePageLoadedAsync();

            // Assert
            Assert.That(isLoaded, Is.True, "Home page should load successfully");
            Assert.That(_homePage.GetCurrentUrl(), Contains.Substring("localhost:5000"), "URL should contain localhost:5000");

            TestContext.WriteLine("✓ Home page loaded successfully");
        }

        [Test]
        [Description("Verify welcome heading is visible on home page")]
        public async Task TC002_VerifyWelcomeHeadingVisible()
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);

            // Act
            var heading = await _homePage.GetWelcomeHeadingAsync();
            var isVisible = await _homePage.IsHomePageLoadedAsync();

            // Assert
            Assert.That(isVisible, Is.True, "Welcome heading should be visible");
            Assert.That(heading, Is.Not.Null.And.Not.Empty, "Welcome heading should contain text");

            TestContext.WriteLine($"✓ Welcome heading visible: {heading}");
        }

        [Test]
        [Description("Verify navigation menu is present on home page")]
        public async Task TC003_VerifyNavigationMenuPresent()
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);

            // Act
            var isMenuVisible = await _homePage.IsNavigationMenuVisibleAsync();

            // Assert
            Assert.That(isMenuVisible, Is.True, "Navigation menu should be visible");

            TestContext.WriteLine("✓ Navigation menu is present");
        }

        [Test]
        [Description("Verify navigation to Invoices page")]
        public async Task TC004_VerifyNavigateToInvoicesPage()
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);

            // Act
            await _homePage.ClickInvoicesLinkAsync();
            var currentUrl = _homePage.GetCurrentUrl();

            // Assert
            Assert.That(currentUrl, Contains.Substring("Invoices"), "Should navigate to Invoices page");

            TestContext.WriteLine("✓ Successfully navigated to Invoices page");
        }

        [Test]
        [Description("Verify navigation to Privacy page")]
        public async Task TC005_VerifyNavigateToPrivacyPage()
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);

            // Act
            await _homePage.ClickPrivacyLinkAsync();
            var currentUrl = _homePage.GetCurrentUrl();

            // Assert
            Assert.That(currentUrl, Contains.Substring("Privacy"), "Should navigate to Privacy page");

            TestContext.WriteLine("✓ Successfully navigated to Privacy page");
        }

        [Test]
        [Description("Verify page load time is acceptable")]
        [Timeout(15000)] // 15 second timeout for this test
        public async Task TC006_VerifyPageLoadTime()
        {
            // Arrange
            var startTime = System.DateTime.Now;

            // Act
            await _homePage.NavigateToAsync(BaseUrl);
            var isLoaded = await _homePage.IsHomePageLoadedAsync();
            var loadTime = System.DateTime.Now - startTime;

            // Assert
            Assert.That(isLoaded, Is.True, "Page should load successfully");
            Assert.That(loadTime.TotalSeconds, Is.LessThan(10), "Page should load within 10 seconds");

            TestContext.WriteLine($"✓ Page loaded in {loadTime.TotalSeconds:F2} seconds");
        }
    }
}
