using System;
using System.IO;

namespace yuki_qa_automation_tests.Utilities
{
    /// <summary>
    /// Logger utility for test execution logging
    /// Supports multiple log levels and both console and file output
    /// </summary>
    public class Logger
    {
        private readonly string _testName;
        private readonly string _logFilePath;
        private bool _debugEnabled = false;

        public Logger(string testName)
        {
            _testName = testName;
            var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            Directory.CreateDirectory(logDirectory);
            _logFilePath = Path.Combine(logDirectory, $"{DateTime.Now:yyyy-MM-dd}.log");
            
            // Enable debug logging in CI/CD environments
            _debugEnabled = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI")) ||
                           !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TF_BUILD"));
        }

        /// <summary>
        /// Logs a debug message (only in CI/CD or when debug is enabled)
        /// </summary>
        public void Debug(string message)
        {
            if (_debugEnabled)
            {
                LogMessage("DEBUG", message);
            }
        }

        /// <summary>
        /// Logs an information message
        /// </summary>
        public void Info(string message)
        {
            LogMessage("INFO", message);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        public void Warning(string message)
        {
            LogMessage("WARNING", message);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        public void Error(string message)
        {
            LogMessage("ERROR", message);
        }

        private void LogMessage(string level, string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var logEntry = $"[{timestamp}] [{level}] [{_testName}] {message}";

            Console.WriteLine(logEntry);

            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch
            {
                // Silently fail if file write fails
            }
        }
    }
}
