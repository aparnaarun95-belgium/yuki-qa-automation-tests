using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.Utilities
{
    public class ScreenshotHelper
    {
        private readonly IPage _page;
        private readonly string _screenshotDirectory;

        public ScreenshotHelper(IPage page, string screenshotDirectory = "Screenshots")
        {
            _page = page ?? throw new ArgumentNullException(nameof(page));
            _screenshotDirectory = screenshotDirectory;

            if (!System.IO.Directory.Exists(_screenshotDirectory))
                System.IO.Directory.CreateDirectory(_screenshotDirectory);
        }

        public async Task<string> CaptureScreenshotAsync(string testName, string suffix = "")
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var filename = $"{testName}_{timestamp}{suffix}.png";
            var filepath = System.IO.Path.Combine(_screenshotDirectory, filename);

            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = filepath, FullPage = true });
            return filepath;
        }

        public async Task<string> CaptureScreenshotOnFailureAsync(string testName)
        {
            return await CaptureScreenshotAsync(testName, "_FAILURE");
        }
    }
}
