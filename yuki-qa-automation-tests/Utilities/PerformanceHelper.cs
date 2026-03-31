using System;
using System.Diagnostics;

namespace yuki_qa_automation_tests.Utilities
{
    public class PerformanceHelper
    {
        private readonly Stopwatch _stopwatch;

        public PerformanceHelper()
        {
            _stopwatch = new Stopwatch();
        }

        public void Start()
        {
            _stopwatch.Restart();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public TimeSpan GetElapsedTime()
        {
            return _stopwatch.Elapsed;
        }

        public double GetElapsedMilliseconds()
        {
            return _stopwatch.Elapsed.TotalMilliseconds;
        }

        public double GetElapsedSeconds()
        {
            return _stopwatch.Elapsed.TotalSeconds;
        }

        public bool IsWithinThreshold(double thresholdMs)
        {
            return _stopwatch.Elapsed.TotalMilliseconds <= thresholdMs;
        }

        public string GetFormattedElapsedTime()
        {
            if (_stopwatch.Elapsed.TotalSeconds >= 1)
                return $"{_stopwatch.Elapsed.TotalSeconds:F2}s";
            else
                return $"{_stopwatch.Elapsed.TotalMilliseconds:F0}ms";
        }
    }
}
