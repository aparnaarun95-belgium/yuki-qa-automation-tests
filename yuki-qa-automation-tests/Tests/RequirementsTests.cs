using System.Threading.Tasks;
using NUnit.Framework;
using yuki_qa_automation_tests.Base;
using yuki_qa_automation_tests.Pages;

namespace yuki_qa_automation_tests.Tests
{
    [TestFixture]
    [Category("UI")]
    [Category("Requirements")]
    public class RequirementsTests : BaseTest
    {
        private HomePage _homePage;
        private InvoicesPage _invoicesPage;

        [SetUp]
        public async Task TestSetup()
        {
            _homePage = new HomePage(Page, DefaultTimeout);
            _invoicesPage = new InvoicesPage(Page, DefaultTimeout);
            BaseUrl = "http://localhost:5000";
        }

        [Test]
        [Description("Main Requirement: Navigate to all pages using menu, verify invoice sum, verify I634 amount")]
        public async Task REQ001_CompleteRequirementTest()
        {
            // Step 1: Navigate to home page
            TestContext.WriteLine("Step 1: Navigating to home page...");
            await _homePage.NavigateToAsync(BaseUrl);
            var isHomePageLoaded = await _homePage.IsHomePageLoadedAsync();
            Assert.That(isHomePageLoaded, Is.True, "Home page should load successfully");
            TestContext.WriteLine("? Home page loaded successfully");

            // Step 2: Verify navigation menu is present
            TestContext.WriteLine("Step 2: Verifying navigation menu...");
            var isNavMenuVisible = await _homePage.IsNavigationMenuVisibleAsync();
            Assert.That(isNavMenuVisible, Is.True, "Navigation menu should be visible");
            TestContext.WriteLine("? Navigation menu is present");

            // Step 3: Navigate to Invoices page using menu
            TestContext.WriteLine("Step 3: Navigating to Invoices page using menu...");
            await _homePage.ClickInvoicesLinkAsync();
            var isInvoicesPageLoaded = await _invoicesPage.IsInvoicesPageLoadedAsync();
            Assert.That(isInvoicesPageLoaded, Is.True, "Invoices page should load successfully");
            TestContext.WriteLine("? Successfully navigated to Invoices page via menu");

            // Step 4: Verify invoice table is visible
            TestContext.WriteLine("Step 4: Verifying invoice table is visible...");
            var isTableVisible = await _invoicesPage.IsInvoiceTableVisibleAsync();
            Assert.That(isTableVisible, Is.True, "Invoice table should be visible");
            TestContext.WriteLine("? Invoice table is visible");

            // Step 5: Verify the sum of all invoices in summary row
            TestContext.WriteLine("Step 5: Verifying sum of all invoices in summary row...");
            var summaryAmount = await _invoicesPage.GetSummaryAmountAsync();
            Assert.That(summaryAmount, Is.EqualTo("963.97 EUR"), "Summary total should be 963.97 EUR");
            TestContext.WriteLine($"? Invoice summary total verified: {summaryAmount}");

            // Step 6: Retrieve and verify specific invoice I634 amount
            TestContext.WriteLine("Step 6: Retrieving and verifying invoice I634 amount...");
            var i634Amount = await _invoicesPage.GetInvoiceAmountByNumberAsync("I634");
            Assert.That(i634Amount, Is.EqualTo("423.99 EUR"), "Invoice I634 should have amount 423.99 EUR");
            TestContext.WriteLine($"? Invoice I634 amount verified: {i634Amount}");

            // Step 7: Navigate back to Home using menu
            TestContext.WriteLine("Step 7: Navigating back to Home page using menu...");
            await _homePage.ClickHomeAsync();
            var isBackAtHome = await _homePage.IsHomePageLoadedAsync();
            Assert.That(isBackAtHome, Is.True, "Should be back at home page");
            TestContext.WriteLine("? Successfully navigated back to Home page via menu");

            // Step 8: Navigate to Privacy page to verify all pages are accessible
            TestContext.WriteLine("Step 8: Navigating to Privacy page...");
            await _homePage.ClickPrivacyLinkAsync();
            var currentUrl = _homePage.GetCurrentUrl();
            Assert.That(currentUrl, Contains.Substring("Privacy"), "Should navigate to Privacy page");
            TestContext.WriteLine("? Successfully navigated to Privacy page");

            // Step 9: Summary - all requirements verified
            TestContext.WriteLine("\n??? ALL REQUIREMENTS VERIFIED ???");
            TestContext.WriteLine("? Navigated to all pages using menu (Home ? Invoices ? Home ? Privacy)");
            TestContext.WriteLine($"? Verified invoice sum in summary row: {summaryAmount}");
            TestContext.WriteLine($"? Verified invoice I634 amount: {i634Amount}");
        }

        [Test]
        [Description("Verify application is flexible for future changes - all invoices can be retrieved")]
        public async Task REQ002_VerifyFlexibilityForFutureChanges()
        {
            // Navigate to invoices page
            await _invoicesPage.NavigateToAsync($"{BaseUrl}/Home/Invoices");

            // Retrieve all invoices (future-proof: will work even if invoices change)
            TestContext.WriteLine("Retrieving all invoices (future-proof retrieval)...");
            var allInvoices = await _invoicesPage.GetAllInvoicesAsync();

            // Verify we have data
            Assert.That(allInvoices, Is.Not.Empty, "Should retrieve invoice data");
            TestContext.WriteLine($"? Retrieved {allInvoices.Count} invoices");

            // Verify each invoice has an amount
            foreach (var invoice in allInvoices)
            {
                Assert.That(invoice.Value, Is.Not.Null.And.Not.Empty, $"Invoice {invoice.Key} should have an amount");
                TestContext.WriteLine($"  - {invoice.Key}: {invoice.Value}");
            }

            TestContext.WriteLine("\n? Application structure is flexible for future changes");
            TestContext.WriteLine("? New invoices can be added/removed without test modifications");
            TestContext.WriteLine("? Code reuses generic retrieval methods for scalability");
        }
    }
}
