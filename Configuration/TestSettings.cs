using Microsoft.Extensions.Configuration;
using System;

namespace yuki_qa_automation_tests.Configuration
{
    /// <summary>
    /// Test settings loaded from appsettings.json
    /// </summary>
    public class TestSettings
    {
        public string BaseUrl { get; set; }
        public BrowserConfig BrowserConfig { get; set; }
        public int PageLoadTimeout { get; set; }
        public int ElementWaitTimeout { get; set; }
        public int NavigationTimeout { get; set; }
        public int RetryAttempts { get; set; }
        public int RetryDelayMs { get; set; }

        public static TestSettings LoadFromConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection("TestSettings").Get<TestSettings>() ?? new TestSettings
            {
                BaseUrl = "http://localhost:5000/",
                BrowserConfig = new BrowserConfig(),
                PageLoadTimeout = 30000,
                ElementWaitTimeout = 10000,
                NavigationTimeout = 30000,
                RetryAttempts = 3,
                RetryDelayMs = 1000
            };
        }
    }
}
