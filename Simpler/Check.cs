using System;

namespace Simpler
{
    public class Check
    {
        public static void That(bool condition, string errorMessage, params object[] args)
        {
            if (errorMessage == null)
            {
                errorMessage = string.Empty;
            }
            else if (args != null && args.Length > 0)
            {
                errorMessage = string.Format(errorMessage, args);
            }

            if (!condition) throw new CheckException(errorMessage);
        }

        public static void Throws(Action action)
        {
            try
            {
                action();
                throw new CheckException("Expected exception to be thrown.");
            }
            catch
            {
            }
        }

        public static void Throws<TException>(Action action)
        {
            var expectedExpection = typeof(TException).FullName;
            try
            {
                action();
                throw new CheckException(String.Format("Expected {0} to be thrown.", expectedExpection));
            }
            catch (Exception exception)
            {
                var actualException = exception.GetType().FullName;
                if (actualException != expectedExpection)
                {
                    throw new CheckException(
                        String.Format("Expected {0} to be thrown, but {1} was thrown instead.",
                                      expectedExpection,
                                      actualException));
                }
            }
        }
    }
}
