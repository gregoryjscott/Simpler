namespace Simpler.Testing
{
    /// <summary>
    /// Method to call to setup the task.
    /// </summary>
    /// <param name="task">The task.</param>
    public delegate void Setup(Task task);

    /// <summary>
    /// Method to verify the task worked, usually using nUnit Assert calls.
    /// </summary>
    /// <param name="task">The task.</param>
    public delegate void Verify(Task task);

    /// <summary>
    /// Contains the information necessary to perform a test on a task.  These should be defined
    /// in a task.DefineTests() method.
    /// </summary>
    public class Test
    {
        public string Expectation { get; set; }
        public Setup Setup { get; set; }
        public Setup Verify { get; set; }
    }
}