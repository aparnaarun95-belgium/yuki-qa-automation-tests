using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.Utilities
{
    public class RetryHelper
    {
        private readonly int _maxRetries;
        private readonly int _delayMs;

        public RetryHelper(int maxRetries = 3, int delayMs = 1000)
        {
            _maxRetries = maxRetries;
            _delayMs = delayMs;
        }

        public async Task<T> RetryAsync<T>(Func<Task<T>> action, string actionName = "")
        {
            List<Exception> exceptions = new();

            for (int attempt = 1; attempt <= _maxRetries; attempt++)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);

                    if (attempt < _maxRetries)
                    {
                        await Task.Delay(_delayMs);
                    }
                }
            }

            throw new AggregateException(
                $"Action '{actionName}' failed after {_maxRetries} retries.",
                exceptions
            );
        }

        public async Task RetryAsync(Func<Task> action, string actionName = "")
        {
            List<Exception> exceptions = new();

            for (int attempt = 1; attempt <= _maxRetries; attempt++)
            {
                try
                {
                    await action();
                    return;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);

                    if (attempt < _maxRetries)
                    {
                        await Task.Delay(_delayMs);
                    }
                }
            }

            throw new AggregateException(
                $"Action '{actionName}' failed after {_maxRetries} retries.",
                exceptions
            );
        }
    }
}
