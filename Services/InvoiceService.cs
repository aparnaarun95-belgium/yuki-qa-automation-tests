using yuki_qa_automation_tests.PageObjects.Models;
using yuki_qa_automation_tests.PageObjects.Pages;
using yuki_qa_automation_tests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.Services
{
    /// <summary>
    /// Service for invoice-related operations
    /// </summary>
    public class InvoiceService
    {
        private readonly InvoicesPage _invoicesPage;
        private readonly Logger _logger;

        public InvoiceService(InvoicesPage invoicesPage, Logger logger)
        {
            _invoicesPage = invoicesPage ?? throw new ArgumentNullException(nameof(invoicesPage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Verifies that the summary total matches the sum of all invoices
        /// </summary>
        public async Task<bool> VerifySummaryTotalAsync()
        {
            _logger.Info("Verifying invoice summary total");

            var invoices = await _invoicesPage.GetAllInvoices();
            var calculatedTotal = invoices.Sum(i => i.Amount);
            var summaryTotal = await _invoicesPage.GetSummaryTotal();

            var isValid = summaryTotal == calculatedTotal;
            _logger.Info($"Summary verification: {(isValid ? "PASSED" : "FAILED")} " +
                         $"(Expected: {calculatedTotal}, Actual: {summaryTotal})");

            return isValid;
        }

        /// <summary>
        /// Verifies an invoice amount
        /// </summary>
        public async Task<bool> VerifyInvoiceAmountAsync(string invoiceId, decimal expectedAmount)
        {
            _logger.Info($"Verifying invoice {invoiceId} amount");

            var actualAmount = await _invoicesPage.GetInvoiceAmount(invoiceId);
            var isValid = actualAmount == expectedAmount;

            _logger.Info($"Invoice {invoiceId} verification: {(isValid ? "PASSED" : "FAILED")} " +
                         $"(Expected: {expectedAmount}, Actual: {actualAmount})");

            return isValid;
        }

        /// <summary>
        /// Gets all invoices
        /// </summary>
        public async Task<List<InvoiceRow>> GetAllInvoicesAsync()
        {
            return await _invoicesPage.GetAllInvoices();
        }
    }
}
