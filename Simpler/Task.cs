using Simpler.Testing;

namespace Simpler
{
    /// <summary>
    /// The foundation of Simpler.
    /// </summary>
    public abstract class Task
    {
        /// <summary>
        /// Container for the data the task needs to execute.
        /// </summary>
        /// <example>
        /// task.Inputs.Question = "Is this cool?";
        /// </example>
        public virtual dynamic Inputs { get; set; }

        /// <summary>
        /// Container for the data the task produces.
        /// </summary>
        /// <example>
        /// var answer = task.Outputs.Answer;
        /// </example>
        public virtual dynamic Outputs { get; set; }

        /// <summary>
        /// Code that uses the Inputs to produce the Outputs.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Shorthand method for setting the inputs, executing, and receiving the outputs in one line of code.
        /// </summary>
        /// <example>
        /// var answer = task.Execute(new { Question = "Is this cool?" }).Outputs.Answer;
        /// </example>
        /// <param name="inputs">Used to set Inputs prior to execution.</param>
        /// <returns>The Outputs</returns>
        public Task Execute(dynamic inputs)
        {
            Inputs = inputs;
            Execute();
            return this;
        }
    }

    public abstract class TaskWithTestsFor<T> : Task where T : Task
    {
        public abstract TestFor<T>[] Tests { get; }
    }
}