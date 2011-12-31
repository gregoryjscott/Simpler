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
        public virtual dynamic Inputs { get; set; }

        /// <summary>
        /// Container for the data the task produces.
        /// </summary>
        public virtual dynamic Outputs { get; set; }

        /// <summary>
        /// Contains the logic that uses the Inputs to produce the Outputs.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Shorthand method for setting the Inputs, executing, and receiving the outputs in one line of code.
        /// </summary>
        /// <param name="inputs">Used to set Inputs prior to execution.</param>
        /// <returns>The Task that allows for .Outputs.</returns>
        public Task Execute(dynamic inputs)
        {
            Inputs = inputs;
            Execute();
            return this;
        }

        /// <summary>
        /// Should return a collection of unit tests for this Task.
        /// </summary>
        /// <returns>Array of unit tests.</returns>
        public virtual Test[] Tests()
        {
            return new Test[0];
        }
    }
}