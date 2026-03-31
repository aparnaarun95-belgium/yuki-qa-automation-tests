using NUnit.Framework;
using System.Threading.Tasks;
using yuki_qa_automation_tests.Base;
using yuki_qa_automation_tests.Pages;

namespace yuki_qa_automation_tests.Tests
{
    [TestFixture]
    [Category("UI")]
    [Category("SignIn")]
    public class SignInTests : BaseTest
    {
        private HomePage _homePage;

        [SetUp]
        public async Task TestSetup()
        {
            _homePage = new HomePage(Page, DefaultTimeout);
            BaseUrl = "https://example.com"; // Replace with actual base URL
        }

        [Test]
        [Description("Verify sign in button is clickable from home page")]
        public async Task TC011_VerifySignInButtonClickable()
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);
            var isSignInVisible = await _homePage.IsSignInButtonVisibleAsync();

            // Act & Assert
            Assert.That(isSignInVisible, Is.True, "Sign in button should be visible before clicking");

            try
            {
                await _homePage.ClickSignInAsync();
                TestContext.WriteLine("? Sign in button clicked successfully");
            }
            catch (System.Exception ex)
            {
                Assert.Fail($"Failed to click sign in button: {ex.Message}");
            }
        }

        [Test]
        [Description("Verify sign up button is clickable from home page")]
        public async Task TC012_VerifySignUpButtonClickable()
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);
            var isSignUpVisible = await _homePage.IsSignUpButtonVisibleAsync();

            // Act & Assert
            Assert.That(isSignUpVisible, Is.True, "Sign up button should be visible before clicking");

            try
            {
                await _homePage.ClickSignUpAsync();
                TestContext.WriteLine("? Sign up button clicked successfully");
            }
            catch (System.Exception ex)
            {
                Assert.Fail($"Failed to click sign up button: {ex.Message}");
            }
        }

        [Test]
        [Description("Verify search functionality works")]
        public async Task TC013_VerifySearchFunctionality()
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);
            var isSearchPresent = await _homePage.IsSearchBoxPresentAsync();
            var searchQuery = "test query";

            // Act & Assert
            Assert.That(isSearchPresent, Is.True, "Search box should be present");

            try
            {
                await _homePage.SearchAsync(searchQuery);
                TestContext.WriteLine($"? Search performed with query: {searchQuery}");
            }
            catch (System.Exception ex)
            {
                Assert.Fail($"Failed to perform search: {ex.Message}");
            }
        }
    }
}
