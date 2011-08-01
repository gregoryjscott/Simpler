namespace Simpler
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
    /// Contains the information necesary to perform a test on a task.  These should be created
    /// and added from the Task.Tests() method.
    /// 
    /// todo - this isn't done
    /// </summary>
    public class Test
    {
        public string Expectation { get; set; }
        public Setup Setup { get; set; }
        public Setup Verify { get; set; }

        // todo - move this to another class, i.e. the one that will fire off the tests?
        public static void Add(Test test)
        {
            throw new System.NotImplementedException();
        }
    }
}