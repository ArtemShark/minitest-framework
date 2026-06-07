using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService
{
    public static class Assert
    {
        public static void ThrowsException<TException>(Action action, string message = "") where TException : Exception
        {
            try
            {
                action();
                throw new AssertionException($"Expected exception type:<{typeof(TException)}> but no exception was thrown. {message}");
            }
            catch (TException)
            {

            }
            catch (Exception ex)
            {
                throw new AssertionException($"Expected exception type:<{typeof(TException)}>. Actual exception type:<{ex.GetType()}>. {message}");
            }
        }

        public static void AreEqual<T>(T? expected, T? actual, string message = "")
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
            {
                throw new AssertionException($"Expected: {expected?.ToString() ?? "null"}. Actual: {actual?.ToString() ?? "null"}. {message}");
            }
        }

        public static void AreNotEqual<T>(T? notExpected, T? actual, string message = "")
        {
            if (EqualityComparer<T>.Default.Equals(notExpected, actual))
            {
                throw new AssertionException($"Expected: {notExpected?.ToString() ?? "null"}. Actual: {actual?.ToString() ?? "null"}. {message}");
            }
        }

        public static void IsTrue(bool condition, string message = "")
        {
            if (!condition)
            {
                throw new AssertionException(message);
            }
        }

        public static void IsFalse(bool condition, string message = "")
        {
            if (condition)
            {
                throw new AssertionException(message);
            }
        }

        public static void Fail(string message = "")
        { 
            throw new AssertionException(message);
        }
    }
}
