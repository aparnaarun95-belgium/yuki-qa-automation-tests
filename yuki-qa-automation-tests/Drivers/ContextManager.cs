using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.Drivers
{
    public class ContextManager : IAsyncDisposable
    {
        private IBrowserContext _context;
        private readonly IBrowser _browser;
        private readonly int _timeout;
        private readonly int _navigationTimeout;

        public IBrowserContext Context => _context;

        public ContextManager(IBrowser browser, int timeout = 30000, int navigationTimeout = 30000)
        {
            _browser = browser ?? throw new ArgumentNullException(nameof(browser));
            _timeout = timeout;
            _navigationTimeout = navigationTimeout;
        }

        public async Task<IBrowserContext> CreateContextAsync(BrowserNewContextOptions options = null)
        {
            var contextOptions = options ?? new BrowserNewContextOptions();
            contextOptions.ViewportSize = new ViewportSize { Width = 1920, Height = 1080 };

            _context = await _browser.NewContextAsync(contextOptions);
            _context.SetDefaultTimeout(_timeout);
            _context.SetDefaultNavigationTimeout(_navigationTimeout);

            return _context;
        }

        public async Task<IPage> CreatePageAsync()
        {
            if (_context == null)
                throw new InvalidOperationException("Context not created. Call CreateContextAsync first.");

            return await _context.NewPageAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.CloseAsync();
                _context = null;
            }
        }
    }
}
