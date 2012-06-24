using System;

namespace Simpler
{
    public class Check
    {
        public static void That(bool condition, string errorMessage)
        {
            if (!condition) throw new SimplerException(errorMessage);
        }

        public static void Throws(Action action)
        {
            try
            {
                action();
                throw new SimplerException("Expected exception to be thrown.");
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
                throw new SimplerException(String.Format("Expected {0} to be thrown.", expectedExpection));
            }
            catch (Exception exception)
            {
                var actualException = exception.GetType().FullName;
                if (actualException != expectedExpection)
                {
                    throw new SimplerException(
                        String.Format("Expected {0} to be thrown, but {1} was thrown instead.",
                                      expectedExpection,
                                      actualException));
                }
            }
        }
    }
}
