using Microsoft.Extensions.Configuration;
using System.IO;

namespace yuki_qa_automation_tests.Configuration
{
    public class PlaywrightConfiguration
    {
        private readonly IConfiguration _configuration;

        public PlaywrightConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public string BaseUrl => _configuration["PlaywrightSettings:BaseUrl"] ?? "http://localhost:5000";
        public string BrowserType => _configuration["PlaywrightSettings:BrowserType"] ?? "chromium";
        public bool Headless => bool.Parse(_configuration["PlaywrightSettings:Headless"] ?? "true");
        public int SlowMo => int.Parse(_configuration["PlaywrightSettings:SlowMo"] ?? "0");
        public int Timeout => int.Parse(_configuration["PlaywrightSettings:Timeout"] ?? "30000");
        public int NavigationTimeout => int.Parse(_configuration["PlaywrightSettings:NavigationTimeout"] ?? "30000");
        public bool ScreenshotOnFailure => bool.Parse(_configuration["PlaywrightSettings:ScreenshotOnFailure"] ?? "true");
        public bool VideoOnFailure => bool.Parse(_configuration["PlaywrightSettings:VideoOnFailure"] ?? "false");
        public bool TraceOnFailure => bool.Parse(_configuration["PlaywrightSettings:TraceOnFailure"] ?? "false");
        public int MaxRetries => int.Parse(_configuration["RetryPolicy:MaxRetries"] ?? "3");
        public int RetryDelayMs => int.Parse(_configuration["RetryPolicy:DelayMilliseconds"] ?? "1000");
        public int DefaultWaitTimeMs => int.Parse(_configuration["Waits:DefaultWaitTimeMs"] ?? "5000");
        public int ElementWaitTimeMs => int.Parse(_configuration["Waits:ElementWaitTimeMs"] ?? "10000");
        public int NavigationWaitTimeMs => int.Parse(_configuration["Waits:NavigationWaitTimeMs"] ?? "30000");
    }
}
