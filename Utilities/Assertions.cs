using System;
using System.Collections.Generic;
using System.Linq;

namespace yuki_qa_automation_tests.Utilities
{
    /// <summary>
    /// Custom assertion helper methods
    /// </summary>
    public static class Assertions
    {
        /// <summary>
        /// Verifies that actual value equals expected value
        /// </summary>
        public static void AreEqual<T>(T expected, T actual, string message = "")
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
            {
                throw new AssertionException(
                    $"Assertion failed. Expected: {expected}, Actual: {actual}. {message}");
            }
        }

        /// <summary>
        /// Verifies that actual value is not equal to unexpected value
        /// </summary>
        public static void AreNotEqual<T>(T unexpected, T actual, string message = "")
        {
            if (EqualityComparer<T>.Default.Equals(unexpected, actual))
            {
                throw new AssertionException(
                    $"Assertion failed. Value should not be: {unexpected}. {message}");
            }
        }

        /// <summary>
        /// Verifies that condition is true
        /// </summary>
        public static void IsTrue(bool condition, string message = "")
        {
            if (!condition)
            {
                throw new AssertionException($"Assertion failed. Expected True but got False. {message}");
            }
        }

        /// <summary>
        /// Verifies that condition is false
        /// </summary>
        public static void IsFalse(bool condition, string message = "")
        {
            if (condition)
            {
                throw new AssertionException($"Assertion failed. Expected False but got True. {message}");
            }
        }

        /// <summary>
        /// Verifies that list contains element
        /// </summary>
        public static void Contains<T>(IEnumerable<T> collection, T element, string message = "")
        {
            if (!collection.Contains(element))
            {
                throw new AssertionException($"Assertion failed. Collection does not contain: {element}. {message}");
            }
        }

        /// <summary>
        /// Verifies that string is not null or empty
        /// </summary>
        public static void IsNotNullOrEmpty(string value, string message = "")
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AssertionException($"Assertion failed. String is null or empty. {message}");
            }
        }
    }

    /// <summary>
    /// Custom assertion exception
    /// </summary>
    public class AssertionException : Exception
    {
        public AssertionException(string message) : base(message)
        {
        }
    }
}
