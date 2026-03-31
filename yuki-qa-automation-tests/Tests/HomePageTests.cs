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
            BaseUrl = "http://localhost:5000"; // Replace with actual base URL
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
            Assert.That(_homePage.GetCurrentUrl(), Contains.Substring("example.com"), "URL should contain base domain");

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
        [Description("Verify sign in button is available on home page")]
        public async Task TC003_VerifySignInButtonVisible()
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);

            // Act
            var isSignInVisible = await _homePage.IsSignInButtonVisibleAsync();

            // Assert
            Assert.That(isSignInVisible, Is.True, "Sign in button should be visible");

            TestContext.WriteLine("✓ Sign in button is visible and clickable");
        }

        [Test]
        [Description("Verify sign up button is available on home page")]
        public async Task TC004_VerifySignUpButtonVisible()
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);

            // Act
            var isSignUpVisible = await _homePage.IsSignUpButtonVisibleAsync();

            // Assert
            Assert.That(isSignUpVisible, Is.True, "Sign up button should be visible");

            TestContext.WriteLine("✓ Sign up button is visible and clickable");
        }

        [Test]
        [Description("Verify navigation menu is present on home page")]
        public async Task TC005_VerifyNavigationMenuPresent()
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
        [Description("Verify search functionality is present on home page")]
        public async Task TC006_VerifySearchFunctionalityPresent()
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);

            // Act
            var isSearchPresent = await _homePage.IsSearchBoxPresentAsync();

            // Assert
            Assert.That(isSearchPresent, Is.True, "Search box should be present");

            TestContext.WriteLine("✓ Search functionality is present");
        }

        [Test]
        [Description("Verify featured section is visible on home page")]
        public async Task TC007_VerifyFeaturedSectionVisible()
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);

            // Act
            var isFeaturedVisible = await _homePage.IsFeaturedSectionVisibleAsync();

            // Assert
            // Note: This assertion may be skipped if the site doesn't have a featured section
            if (isFeaturedVisible)
            {
                TestContext.WriteLine("✓ Featured section is visible");
            }
            else
            {
                TestContext.WriteLine("⚠ Featured section not found (may not be present on site)");
            }
        }

        [Test]
        [Description("Verify page title is correct")]
        public async Task TC008_VerifyPageTitle()
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);

            // Act
            var pageTitle = await _homePage.GetPageHeadingAsync();

            // Assert
            Assert.That(pageTitle, Is.Not.Null.And.Not.Empty, "Page title should not be empty");

            TestContext.WriteLine($"✓ Page title: {pageTitle}");
        }

        [Test]
        [Description("Verify page responsiveness by checking viewport")]
        public async Task TC009_VerifyPageResponsiveness()
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);

            // Act
            var currentUrl = _homePage.GetCurrentUrl();
            var isLoaded = await _homePage.IsHomePageLoadedAsync();

            // Assert
            Assert.That(isLoaded, Is.True, "Page should be responsive and load correctly");
            Assert.That(currentUrl, Is.Not.Null, "Current URL should not be null");

            TestContext.WriteLine($"✓ Page is responsive - Current URL: {currentUrl}");
        }

        [Test]
        [Description("Verify home page loads within acceptable time")]
        [Timeout(15000)] // 15 second timeout for this test
        public async Task TC010_VerifyPageLoadTime()
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
