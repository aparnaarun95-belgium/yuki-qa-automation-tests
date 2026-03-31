using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using yuki_qa_automation_tests.Base;
using yuki_qa_automation_tests.Pages;

namespace yuki_qa_automation_tests.Tests
{
    [TestFixture]
    [Category("UI")]
    [Category("DataDriven")]
    public class DataDrivenTests : BaseTest
    {
        private HomePage _homePage;

        [SetUp]
        public async Task TestSetup()
        {
            _homePage = new HomePage(Page, DefaultTimeout);
            BaseUrl = "https://example.com"; // Replace with actual base URL
        }

        [Test]
        [TestCaseSource(nameof(GetSearchTestData))]
        [Description("Verify search works with multiple search terms")]
        public async Task TC014_VerifySearchWithMultipleTerms(string searchTerm, string expectedResult)
        {
            // Arrange
            await _homePage.NavigateToAsync(BaseUrl);
            var isSearchPresent = await _homePage.IsSearchBoxPresentAsync();

            // Act & Assert
            Assert.That(isSearchPresent, Is.True, "Search box should be present");

            try
            {
                await _homePage.SearchAsync(searchTerm);
                TestContext.WriteLine($"? Search executed for: {searchTerm}");
            }
            catch (System.Exception ex)
            {
                Assert.Fail($"Failed to execute search: {ex.Message}");
            }
        }

        public static IEnumerable<TestCaseData> GetSearchTestData()
        {
            yield return new TestCaseData("feature", "Features").SetName("Search_Feature");
            yield return new TestCaseData("documentation", "Documentation").SetName("Search_Documentation");
            yield return new TestCaseData("pricing", "Pricing").SetName("Search_Pricing");
            yield return new TestCaseData("support", "Support").SetName("Search_Support");
        }
    }
}
