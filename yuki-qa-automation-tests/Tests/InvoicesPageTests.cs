using System.Threading.Tasks;
using NUnit.Framework;
using yuki_qa_automation_tests.Base;
using yuki_qa_automation_tests.Pages;

namespace yuki_qa_automation_tests.Tests
{
    [TestFixture]
    [Category("UI")]
    [Category("Invoices")]
    public class InvoicesPageTests : BaseTest
    {
        private InvoicesPage _invoicesPage;

        [SetUp]
        public async Task TestSetup()
        {
            _invoicesPage = new InvoicesPage(Page, DefaultTimeout);
            BaseUrl = "http://localhost:5000/Home/Invoices";
        }

        [Test]
        [Description("Verify invoices page loads successfully")]
        public async Task TC011_VerifyInvoicesPageLoads()
        {
            // Arrange & Act
            await _invoicesPage.NavigateToAsync(BaseUrl);
            var isLoaded = await _invoicesPage.IsInvoicesPageLoadedAsync();

            // Assert
            Assert.That(isLoaded, Is.True, "Invoices page should load successfully");
            Assert.That(_invoicesPage.GetCurrentUrl(), Contains.Substring("Invoices"), "URL should contain Invoices");

            TestContext.WriteLine("? Invoices page loaded successfully");
        }

        [Test]
        [Description("Verify invoice table is visible and contains data")]
        public async Task TC012_VerifyInvoiceTableVisible()
        {
            // Arrange & Act
            await _invoicesPage.NavigateToAsync(BaseUrl);
            var isTableVisible = await _invoicesPage.IsInvoiceTableVisibleAsync();
            var rowCount = await _invoicesPage.GetInvoiceRowCountAsync();

            // Assert
            Assert.That(isTableVisible, Is.True, "Invoice table should be visible");
            Assert.That(rowCount, Is.GreaterThan(0), "Invoice table should contain data rows");

            TestContext.WriteLine($"? Invoice table visible with {rowCount} invoices");
        }

        [Test]
        [Description("Verify invoice summary row is correct")]
        public async Task TC013_VerifyInvoiceSummary()
        {
            // Arrange
            await _invoicesPage.NavigateToAsync(BaseUrl);

            // Act
            var summaryAmount = await _invoicesPage.GetSummaryAmountAsync();

            // Assert
            Assert.That(summaryAmount, Is.EqualTo("963.97 EUR"), "Summary total should be 963.97 EUR");

            TestContext.WriteLine($"? Invoice summary verified: {summaryAmount}");
        }

        [Test]
        [Description("Verify specific invoice I634 has correct amount")]
        public async Task TC014_VerifyInvoiceI634Amount()
        {
            // Arrange
            await _invoicesPage.NavigateToAsync(BaseUrl);

            // Act
            var i634Amount = await _invoicesPage.GetInvoiceAmountByNumberAsync("I634");

            // Assert
            Assert.That(i634Amount, Is.EqualTo("423.99 EUR"), "Invoice I634 should have amount 423.99 EUR");

            TestContext.WriteLine($"? Invoice I634 amount verified: {i634Amount}");
        }

        [Test]
        [Description("Verify all invoice rows contain expected data")]
        public async Task TC015_VerifyAllInvoicesVisible()
        {
            // Arrange
            await _invoicesPage.NavigateToAsync(BaseUrl);

            // Act
            var invoices = await _invoicesPage.GetAllInvoicesAsync();

            // Assert
            Assert.That(invoices, Is.Not.Empty, "Invoice list should not be empty");
            Assert.That(invoices.Count, Is.EqualTo(3), "Should have exactly 3 invoices");

            // Verify invoice numbers exist
            var invoiceNumbers = new[] { "I523", "I634", "I125" };
            foreach (var invoiceNumber in invoiceNumbers)
            {
                Assert.That(invoices, Does.ContainKey(invoiceNumber), $"Invoice {invoiceNumber} should exist");
            }

            TestContext.WriteLine($"? All invoices verified: {string.Join(", ", invoices.Keys)}");
        }
    }
}
