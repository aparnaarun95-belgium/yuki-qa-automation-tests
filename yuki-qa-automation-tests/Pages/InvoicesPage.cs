using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace yuki_qa_automation_tests.Pages
{
    /// <summary>
    /// Invoices Page Object Model.
    /// Provides methods to interact with and verify invoice data.
    /// Inherits navigation functionality from NavigablePage to avoid code duplication.
    /// </summary>
    public class InvoicesPage : NavigablePage
    {
        // Selectors for invoice page specific elements
        private const string InvoiceHeading = "h1";
        private const string InvoiceTable = "table";
        private const string TableRows = "table tbody tr";
        private const string SummaryRow = "table tbody tr.summary-row";

        public InvoicesPage(IPage page, int defaultTimeout = 10000)
            : base(page, defaultTimeout)
        {
        }

        /// <summary>
        /// Checks if the invoices page is loaded.
        /// </summary>
        public async Task<bool> IsInvoicesPageLoadedAsync()
        {
            return await IsElementVisibleAsync(InvoiceHeading);
        }

        /// <summary>
        /// Implements the abstract method from NavigablePage.
        /// </summary>
        public override async Task<bool> IsPageLoadedAsync()
        {
            return await IsInvoicesPageLoadedAsync();
        }

        public async Task<bool> IsInvoiceTableVisibleAsync()
        {
            return await IsElementVisibleAsync(InvoiceTable);
        }

        public async Task<int> GetInvoiceRowCountAsync()
        {
            try
            {
                var locators = await Page.Locator(TableRows).CountAsync();
                // Subtract 1 if there's a summary row (last row)
                var hasSummaryRow = await Page.Locator(SummaryRow).CountAsync() > 0;
                return hasSummaryRow ? locators - 1 : locators;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Retrieves the summary amount from the summary row.
        /// This is used to verify the total of all invoices.
        /// </summary>
        public async Task<string> GetSummaryAmountAsync()
        {
            try
            {
                var summaryRowCells = Page.Locator($"{SummaryRow} td");
                var cellCount = await summaryRowCells.CountAsync();

                if (cellCount == 0)
                {
                    throw new InvalidOperationException("Summary row cells not found");
                }

                var lastCell = summaryRowCells.Nth(cellCount - 1);
                var amount = await lastCell.TextContentAsync();
                return amount?.Trim() ?? string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get summary amount: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Retrieves the amount for a specific invoice by invoice number.
        /// Example: GetInvoiceAmountByNumberAsync("I634") returns "423.99 EUR"
        /// </summary>
        public async Task<string> GetInvoiceAmountByNumberAsync(string invoiceNumber)
        {
            try
            {
                // Find the row containing the invoice number and get the amount from the last cell
                var row = Page.Locator($"table tbody tr:has-text('{invoiceNumber}')");
                var cells = row.Locator("td");
                var cellCount = await cells.CountAsync();

                if (cellCount == 0)
                {
                    throw new InvalidOperationException($"Invoice {invoiceNumber} not found");
                }

                var lastCell = cells.Nth(cellCount - 1);
                var amount = await lastCell.TextContentAsync();
                return amount?.Trim() ?? string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get invoice amount for {invoiceNumber}: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Retrieves all invoices from the table as a dictionary.
        /// Returns: Dictionary with invoice number as key and amount as value.
        /// This enables verification of the sum of all invoices.
        /// </summary>
        public async Task<Dictionary<string, string>> GetAllInvoicesAsync()
        {
            var invoices = new Dictionary<string, string>();

            try
            {
                var rows = Page.Locator("table tbody tr:not(.summary-row)");
                var rowCount = await rows.CountAsync();

                for (int i = 0; i < rowCount; i++)
                {
                    var row = rows.Nth(i);
                    var cells = row.Locator("td");
                    var cellCount = await cells.CountAsync();

                    if (cellCount >= 3)
                    {
                        var invoiceNumber = (await cells.Nth(0).TextContentAsync())?.Trim() ?? string.Empty;
                        var amount = (await cells.Nth(2).TextContentAsync())?.Trim() ?? string.Empty;

                        if (!string.IsNullOrEmpty(invoiceNumber) && !string.IsNullOrEmpty(amount))
                        {
                            invoices[invoiceNumber] = amount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get all invoices: {ex.Message}");
            }

            return invoices;
        }

        /// <summary>
        /// Calculates the sum of all invoice amounts.
        /// This method extracts the numeric value and sums all amounts.
        /// </summary>
        public async Task<decimal> CalculateTotalInvoiceAmountAsync()
        {
            try
            {
                var invoices = await GetAllInvoicesAsync();
                decimal total = 0;

                foreach (var amount in invoices.Values)
                {
                    // Extract numeric value from amount string (e.g., "423.99 EUR" -> 423.99)
                    var numericPart = System.Text.RegularExpressions.Regex.Match(amount, @"[\d.]+").Value;
                    if (decimal.TryParse(numericPart, out decimal value))
                    {
                        total += value;
                    }
                }

                return total;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to calculate total: {ex.Message}");
                return 0;
            }
        }
    }
}
