using NUnit.Framework;
using yuki_qa_automation_tests.Core;
using yuki_qa_automation_tests.PageObjects.Pages;
using yuki_qa_automation_tests.Services;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.Tests.Integration
{
    /// <summary>
    /// Integration tests for invoice functionality
    /// </summary>
    [TestFixture]
    public class InvoiceTests : BaseTest
    {
        private MenuPage _menuPage;
        private InvoicesPage _invoicesPage;
        private NavigationService _navigationService;
        private InvoiceService _invoiceService;

        [SetUp]
        public new async Task Setup()
        {
            await base.Setup();

            _menuPage = new MenuPage(Page, TestSettings.BaseUrl, Logger);
            _invoicesPage = new InvoicesPage(Page, TestSettings.BaseUrl, Logger);
            _navigationService = new NavigationService(_menuPage, Logger);
            _invoiceService = new InvoiceService(_invoicesPage, Logger);
        }

        /// <summary>
        /// Test: Navigate to all pages using the menu
        /// </summary>
        [Test]
        public async Task NavigateToAllPages_UsingMenu_ShouldSucceed()
        {
            // Arrange
            await _menuPage.Navigate(TestSettings.BaseUrl);

            // Act
            var result = await _navigationService.NavigateToAllPagesAsync();

            // Assert
            Assert.IsTrue(result, "Navigation through all pages should succeed");
        }

        /// <summary>
        /// Test: Verify the sum of all invoices in the summary row
        /// </summary>
        [Test]
        public async Task VerifyInvoiceSummaryTotal_ShouldMatchSumOfAllInvoices()
        {
            // Arrange
            await _invoicesPage.NavigateToInvoicesPage();

            // Act
            var result = await _invoiceService.VerifySummaryTotalAsync();

            // Assert
            Assert.IsTrue(result, "Invoice summary total should match the sum of all invoices");
        }

        /// <summary>
        /// Test: Retrieve and verify amount for invoice 'I634' is '423.99 EUR'
        /// </summary>
        [Test]
        public async Task VerifyInvoiceAmount_I634_ShouldEqual_423_99_EUR()
        {
            // Arrange
            await _invoicesPage.NavigateToInvoicesPage();
            const string invoiceId = "I634";
            const decimal expectedAmount = 423.99m;

            // Act
            var result = await _invoiceService.VerifyInvoiceAmountAsync(invoiceId, expectedAmount);

            // Assert
            Assert.IsTrue(result, 
                $"Invoice {invoiceId} amount should be {expectedAmount} EUR");
        }
    }
}
