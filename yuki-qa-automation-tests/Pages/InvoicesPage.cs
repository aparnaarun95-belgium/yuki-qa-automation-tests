using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace yuki_qa_automation_tests.Pages
{
    public class InvoicesPage : BasePage
    {
        // Selectors for invoice page elements
        private const string InvoiceHeading = "h1";
        private const string InvoiceTable = "table";
        private const string TableRows = "table tbody tr";
        private const string SummaryRow = "table tbody tr.summary-row";

        public InvoicesPage(IPage page, int defaultTimeout = 10000)
            : base(page, defaultTimeout)
        {
        }

        public async Task<bool> IsInvoicesPageLoadedAsync()
        {
            return await IsElementVisibleAsync(InvoiceHeading);
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

        public async Task<string> GetSummaryAmountAsync()
        {
            try
            {
                var summaryRowCells = Page.Locator($"{SummaryRow} td");
                var lastCell = summaryRowCells.Nth(await summaryRowCells.CountAsync() - 1);
                return (await lastCell.TextContentAsync()).Trim();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get summary amount: {ex.Message}");
                return string.Empty;
            }
        }

        public async Task<string> GetInvoiceAmountByNumberAsync(string invoiceNumber)
        {
            try
            {
                // Find the row containing the invoice number and get the amount from the last cell
                var row = Page.Locator($"table tbody tr:has-text('{invoiceNumber}')");
                var cells = row.Locator("td");
                var lastCell = cells.Nth(await cells.CountAsync() - 1);
                return (await lastCell.TextContentAsync()).Trim();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get invoice amount for {invoiceNumber}: {ex.Message}");
                return string.Empty;
            }
        }

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

                    if (await cells.CountAsync() >= 3)
                    {
                        var invoiceNumber = (await cells.Nth(0).TextContentAsync()).Trim();
                        var amount = (await cells.Nth(2).TextContentAsync()).Trim();
                        invoices[invoiceNumber] = amount;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get all invoices: {ex.Message}");
            }

            return invoices;
        }
    }
}
