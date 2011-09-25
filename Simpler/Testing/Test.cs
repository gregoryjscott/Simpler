namespace Simpler.Testing
{
    /// <summary>
    /// Method to call to setup the task.
    /// </summary>
    /// <param name="task">The task.</param>
    public delegate void SetupFor<T>(T task);

    /// <summary>
    /// Method to verify the task worked, usually using nUnit Assert calls.
    /// </summary>
    /// <param name="task">The task.</param>
    public delegate void VerificationFor<T>(T task);

    /// <summary>
    /// Contains the information necessary to perform a test on a task.  These should be defined
    /// in a task.DefineTests() method.
    /// </summary>
    public class TestFor<T>
    {
        public string Expectation { get; set; }
        public SetupFor<T> Setup { get; set; }
        public VerificationFor<T> Verify { get; set; }
    }
}