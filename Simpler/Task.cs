namespace Simpler
{
    /// <summary>
    /// The foundation of Simpler.
    /// </summary>
    public abstract class Task
    {
        public virtual dynamic Inputs { get; set; }

        public virtual dynamic Outputs { get; set; }

        public abstract void Execute();

        public Task Execute(dynamic inputs)
        {
            Inputs = inputs;
            Execute();
            return this;
        }
    }
}