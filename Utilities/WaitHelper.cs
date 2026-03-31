using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.Utilities
{
    /// <summary>
    /// Helper class for wait operations with resilience patterns
    /// Optimized for CI/CD environments with intelligent retry and timeout handling
    /// </summary>
    public class WaitHelper
    {
        private readonly IPage _page;
        private readonly Logger _logger;
        private const int DefaultTimeoutMs = 10000;

        public WaitHelper(IPage page, Logger logger)
        {
            _page = page ?? throw new ArgumentNullException(nameof(page));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Waits for page to load with fallback to DOMContentLoaded
        /// Optimized for network latency in CI/CD environments
        /// </summary>
        public async Task WaitForPageLoadAsync(int timeoutMs = DefaultTimeoutMs)
        {
            try
            {
                _logger.Debug($"Waiting for page load with timeout {timeoutMs}ms");
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions { Timeout = timeoutMs });
                _logger.Debug("Page load completed (NetworkIdle)");
            }
            catch (PlaywrightException ex)
            {
                _logger.Warning($"NetworkIdle timeout: {ex.Message}. Attempting DOMContentLoaded fallback...");
                try
                {
                    await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded, new PageWaitForLoadStateOptions { Timeout = timeoutMs / 2 });
                    _logger.Debug("Page load completed (DOMContentLoaded fallback)");
                }
                catch (Exception fallbackEx)
                {
                    _logger.Error($"Both NetworkIdle and DOMContentLoaded waits failed: {fallbackEx.Message}");
                    // Don't throw - continue as page might be functionally ready
                }
            }
        }

        /// <summary>
        /// Waits for an element to be visible with retry logic
        /// </summary>
        public async Task WaitForElementAsync(string selector, int timeoutMs = DefaultTimeoutMs, int retries = 1)
        {
            if (string.IsNullOrWhiteSpace(selector))
                throw new ArgumentException("Selector cannot be null or empty", nameof(selector));

            int attempt = 0;
            while (attempt <= retries)
            {
                try
                {
                    _logger.Debug($"Waiting for element '{selector}' (attempt {attempt + 1}, timeout {timeoutMs}ms)");
                    await _page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions { Timeout = timeoutMs });
                    _logger.Debug($"Element '{selector}' found");
                    return;
                }
                catch (PlaywrightException ex) when (attempt < retries)
                {
                    _logger.Warning($"Element '{selector}' not found on attempt {attempt + 1}. Retrying...");
                    attempt++;
                    await Task.Delay(100); // Brief delay before retry
                }
                catch (PlaywrightException ex)
                {
                    _logger.Error($"Element '{selector}' not found after {retries + 1} attempts within {timeoutMs}ms: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Waits for an element to be hidden
        /// </summary>
        public async Task WaitForElementToHideAsync(string selector, int timeoutMs = DefaultTimeoutMs)
        {
            if (string.IsNullOrWhiteSpace(selector))
                throw new ArgumentException("Selector cannot be null or empty", nameof(selector));

            try
            {
                _logger.Debug($"Waiting for element '{selector}' to hide (timeout {timeoutMs}ms)");
                await _page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions 
                { 
                    Timeout = timeoutMs,
                    State = WaitForSelectorState.Hidden
                });
                _logger.Debug($"Element '{selector}' hidden");
            }
            catch (PlaywrightException ex)
            {
                _logger.Error($"Element '{selector}' did not hide within {timeoutMs}ms: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Waits for a navigation to complete
        /// </summary>
        public async Task WaitForNavigationAsync(int timeoutMs = DefaultTimeoutMs)
        {
            try
            {
                _logger.Debug($"Waiting for navigation (timeout {timeoutMs}ms)");
                await _page.WaitForNavigationAsync(new PageWaitForNavigationOptions { Timeout = timeoutMs });
                _logger.Debug("Navigation completed");
            }
            catch (PlaywrightException ex)
            {
                _logger.Warning($"Navigation timeout after {timeoutMs}ms: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Waits for a function to return true
        /// Useful for custom wait conditions
        /// </summary>
        public async Task WaitForFunctionAsync(string expression, int timeoutMs = DefaultTimeoutMs)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Expression cannot be null or empty", nameof(expression));

            try
            {
                _logger.Debug($"Waiting for function: {expression}");
                await _page.WaitForFunctionAsync(expression, new PageWaitForFunctionOptions { Timeout = timeoutMs });
                _logger.Debug("Function condition met");
            }
            catch (PlaywrightException ex)
            {
                _logger.Error($"Wait for function failed: {ex.Message}");
                throw;
            }
        }
    }
}
