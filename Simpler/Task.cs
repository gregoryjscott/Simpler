namespace Simpler
{
    /// <summary>
    /// Base class for all tasks.  It ensures that all tasks have the Execute() method that
    /// can be overridden (which allows it to be intercepted in a proxy class).
    /// </summary>
    public abstract class Task
    {
        public abstract void Execute();
    }
}