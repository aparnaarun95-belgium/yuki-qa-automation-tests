using Reqnroll;
using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Playwright;
using yuki_qa_automation_tests.Pages;

namespace yuki_qa_automation_tests.StepDefinitions
{
    /// <summary>
    /// Invoice-specific step definitions for Reqnroll BDD tests.
    /// Reusable steps for invoice verification scenarios.
    /// </summary>
    [Binding]
    public class InvoiceSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private IPage _page;
        private InvoicesPage _invoicesPage;

        public InvoiceSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        }

        [Given(@"I navigate to the invoices page")]
        public async Task GivenINavigateToTheInvoicesPage()
        {
            _page = (IPage)_scenarioContext["Page"];
            _invoicesPage = new InvoicesPage(_page, 30000);

            // Navigate via home page first
            var homePage = new HomePage(_page, 30000);
            await homePage.NavigateToAsync("http://localhost:5000");
            await homePage.NavigateToInvoicesAsync();

            Console.WriteLine("Navigated to invoices page");
        }

        [When(@"I wait for the invoices page to load")]
        public async Task WhenIWaitForTheInvoicesPageToLoad()
        {
            // Ensure _invoicesPage is initialized
            if (_invoicesPage == null)
            {
                _page = (IPage)_scenarioContext["Page"];
                _invoicesPage = new InvoicesPage(_page, 30000);
            }

            var isLoaded = await _invoicesPage.IsInvoicesPageLoadedAsync();
            if (!isLoaded)
            {
                throw new InvalidOperationException("Invoices page did not load within the expected time");
            }
            Console.WriteLine("Invoices page loaded successfully");
        }

        [Then(@"the invoices page should be displayed")]
        public async Task ThenTheInvoicesPageShouldBeDisplayed()
        {
            // Ensure _invoicesPage is initialized
            if (_invoicesPage == null)
            {
                _page = (IPage)_scenarioContext["Page"];
                _invoicesPage = new InvoicesPage(_page, 30000);
            }

            var isLoaded = await _invoicesPage.IsInvoicesPageLoadedAsync();
            Assert.That(isLoaded, Is.True, "Invoices page is not displayed");
            Console.WriteLine("? Invoices page is displayed");
        }

        [Then(@"the invoice table should be visible")]
        public async Task ThenTheInvoiceTableShouldBeVisible()
        {
            // Ensure _invoicesPage is initialized
            if (_invoicesPage == null)
            {
                _page = (IPage)_scenarioContext["Page"];
                _invoicesPage = new InvoicesPage(_page, 30000);
            }

            var isVisible = await _invoicesPage.IsInvoiceTableVisibleAsync();
            Assert.That(isVisible, Is.True, "Invoice table is not visible");
            Console.WriteLine("? Invoice table is visible");
        }

        /// <summary>
        /// Verifies the sum of all invoices matches the summary amount.
        /// This step calculates the total from all invoice rows and compares it with the summary row.
        /// </summary>
        [Then(@"the sum of all invoices should match the summary amount")]
        public async Task ThenTheSumOfAllInvoicesShouldMatchTheSummaryAmount()
        {
            // Ensure _invoicesPage is initialized
            if (_invoicesPage == null)
            {
                _page = (IPage)_scenarioContext["Page"];
                _invoicesPage = new InvoicesPage(_page, 30000);
            }

            var calculatedTotal = await _invoicesPage.CalculateTotalInvoiceAmountAsync();
            var summaryAmount = await _invoicesPage.GetSummaryAmountAsync();

            // Extract numeric value from summary amount (e.g., "1234.56 EUR" -> 1234.56)
            var numericPart = System.Text.RegularExpressions.Regex.Match(summaryAmount, @"[\d.]+").Value;

            if (!decimal.TryParse(numericPart, out decimal summaryValue))
            {
                throw new InvalidOperationException($"Could not parse summary amount: {summaryAmount}");
            }

            Assert.That(calculatedTotal, Is.EqualTo(summaryValue).Within(0.01m),
                $"Sum of invoices ({calculatedTotal}) does not match summary amount ({summaryValue})");

            Console.WriteLine($"? Invoice sum verified: {calculatedTotal} = {summaryValue}");
        }

        /// <summary>
        /// Retrieves and verifies a specific invoice amount by invoice number.
        /// Example: invoice number "I634" should have amount "423.99 EUR"
        /// </summary>
        [Then(@"the invoice ""(.*)"" should have amount ""(.*)""")]
        public async Task ThenTheInvoiceShouldHaveAmount(string invoiceNumber, string expectedAmount)
        {
            // Ensure _invoicesPage is initialized
            if (_invoicesPage == null)
            {
                _page = (IPage)_scenarioContext["Page"];
                _invoicesPage = new InvoicesPage(_page, 30000);
            }

            var actualAmount = await _invoicesPage.GetInvoiceAmountByNumberAsync(invoiceNumber);

            Assert.That(actualAmount, Is.EqualTo(expectedAmount),
                $"Invoice {invoiceNumber}: expected '{expectedAmount}' but got '{actualAmount}'");

            Console.WriteLine($"? Invoice {invoiceNumber} verified: {actualAmount}");
        }

        /// <summary>
        /// Verifies that a specific invoice exists on the invoices page.
        /// </summary>
        [Then(@"invoice ""(.*)"" should exist")]
        public async Task ThenInvoiceShouldExist(string invoiceNumber)
        {
            // Ensure _invoicesPage is initialized
            if (_invoicesPage == null)
            {
                _page = (IPage)_scenarioContext["Page"];
                _invoicesPage = new InvoicesPage(_page, 30000);
            }

            var invoices = await _invoicesPage.GetAllInvoicesAsync();

            Assert.That(invoices.Keys, Does.Contain(invoiceNumber),
                $"Invoice {invoiceNumber} not found in invoices list");

            Console.WriteLine($"? Invoice {invoiceNumber} exists");
        }

        /// <summary>
        /// Verifies the number of invoices on the page.
        /// </summary>
        [Then(@"the invoices page should contain at least (\d+) invoices")]
        public async Task ThenTheInvoicesPageShouldContainAtLeastInvoices(int minimumCount)
        {
            // Ensure _invoicesPage is initialized
            if (_invoicesPage == null)
            {
                _page = (IPage)_scenarioContext["Page"];
                _invoicesPage = new InvoicesPage(_page, 30000);
            }

            var invoices = await _invoicesPage.GetAllInvoicesAsync();

            Assert.That(invoices.Count, Is.GreaterThanOrEqualTo(minimumCount),
                $"Expected at least {minimumCount} invoices but found {invoices.Count}");

            Console.WriteLine($"? Invoice count verified: {invoices.Count} invoices (minimum {minimumCount})");
        }
    }
}
