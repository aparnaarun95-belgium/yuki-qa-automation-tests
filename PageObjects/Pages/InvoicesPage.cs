using Microsoft.Playwright;
using yuki_qa_automation_tests.PageObjects.Models;
using yuki_qa_automation_tests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.PageObjects.Pages
{
    /// <summary>
    /// Page object for Invoices page
    /// </summary>
    public class InvoicesPage : MenuPage
    {
        private const string InvoiceTable = "table.table";
        private const string TableRows = "table.table tbody tr";
        private const string SummaryRow = "table.table tfoot tr";

        public InvoicesPage(IPage page, string baseUrl, Logger logger) : base(page, baseUrl, logger)
        {
        }

        /// <summary>
        /// Navigates to the Invoices page
        /// </summary>
        public async Task NavigateToInvoicesPage()
        {
            Logger.Info("Navigating to Invoices page");
            await Navigate(BaseUrl + "invoices");
        }

        /// <summary>
        /// Retrieves all invoices from the table
        /// </summary>
        public async Task<List<InvoiceRow>> GetAllInvoices()
        {
            Logger.Info("Retrieving all invoices from the table");
            await WaitHelper.WaitForElementAsync(TableRows);

            var invoices = new List<InvoiceRow>();
            var rows = await Page.QuerySelectorAllAsync(TableRows);

            foreach (var row in rows)
            {
                var invoiceIdElement = await row.QuerySelectorAsync("td:nth-child(1)");
                var amountElement = await row.QuerySelectorAsync("td:nth-child(2)");

                if (invoiceIdElement != null && amountElement != null)
                {
                    var invoiceId = await invoiceIdElement.TextContentAsync();
                    var amountText = await amountElement.TextContentAsync();

                    invoiceId = invoiceId?.Trim();
                    amountText = amountText?.Trim();

                    if (!string.IsNullOrEmpty(invoiceId) && invoiceId != "Invoice ID")
                    {
                        invoices.Add(new InvoiceRow
                        {
                            InvoiceId = invoiceId,
                            Amount = ParseAmount(amountText)
                        });
                    }
                }
            }

            Logger.Info($"Retrieved {invoices.Count} invoices");
            return invoices;
        }

        /// <summary>
        /// Gets the summary total from the table footer
        /// </summary>
        public async Task<decimal> GetSummaryTotal()
        {
            Logger.Info("Retrieving summary total from the table");
            var summaryRowElement = await Page.QuerySelectorAsync(SummaryRow);
            if (summaryRowElement == null)
            {
                throw new Exception("Summary row not found");
            }

            var amountElement = await summaryRowElement.QuerySelectorAsync("td:nth-child(2)");
            var summaryText = await amountElement.TextContentAsync();
            var total = ParseAmount(summaryText?.Trim());

            Logger.Info($"Summary total retrieved: {total}");
            return total;
        }

        /// <summary>
        /// Retrieves the amount for a specific invoice ID
        /// </summary>
        public async Task<decimal> GetInvoiceAmount(string invoiceId)
        {
            Logger.Info($"Retrieving amount for invoice: {invoiceId}");
            var rows = await Page.QuerySelectorAllAsync(TableRows);

            foreach (var row in rows)
            {
                var invoiceIdElement = await row.QuerySelectorAsync("td:nth-child(1)");
                var amountElement = await row.QuerySelectorAsync("td:nth-child(2)");

                if (invoiceIdElement != null && amountElement != null)
                {
                    var currentId = await invoiceIdElement.TextContentAsync();
                    currentId = currentId?.Trim();

                    if (currentId == invoiceId)
                    {
                        var amountText = await amountElement.TextContentAsync();
                        var amount = ParseAmount(amountText?.Trim());
                        Logger.Info($"Amount for invoice {invoiceId}: {amount}");
                        return amount;
                    }
                }
            }

            throw new Exception($"Invoice {invoiceId} not found");
        }

        /// <summary>
        /// Parses amount string in format "123.45 EUR"
        /// </summary>
        private decimal ParseAmount(string amountText)
        {
            if (string.IsNullOrEmpty(amountText))
            {
                throw new Exception("Amount text is empty");
            }

            var parts = amountText.Split(' ');
            if (parts.Length > 0 && decimal.TryParse(parts[0], out decimal amount))
            {
                return amount;
            }

            throw new Exception($"Unable to parse amount: {amountText}");
        }
    }
}
