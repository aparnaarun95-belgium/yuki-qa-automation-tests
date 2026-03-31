using System;

namespace yuki_qa_automation_tests.Configuration
{
    /// <summary>
    /// Configuration settings for browser initialization
    /// </summary>
    public class BrowserConfig
    {
        public string BrowserType { get; set; } = "chromium";
        public bool Headless { get; set; } = true;
        public int TimeoutMs { get; set; } = 30000;
        public int NavigationTimeoutMs { get; set; } = 30000;
        public bool RecordVideo { get; set; } = false;
        public string VideoPath { get; set; } = "./videos";
        public bool CaptureScreenshot { get; set; } = true;
        public string ScreenshotPath { get; set; } = "./screenshots";
        public int SlowMo { get; set; } = 0;
        public string[] Args { get; set; } = new string[] { };
        public System.Collections.Generic.Dictionary<string, string> EnvironmentVariables { get; set; } = new();
    }
}
