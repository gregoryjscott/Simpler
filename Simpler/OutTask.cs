using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class OutTask<TOutputs> : Task
    {
        public virtual TOutputs Outputs { get; set; }

        public virtual dynamic GetOutputs()
        {
            return Outputs;
        }

        public virtual OutTask<TOutputs> DoIt()
        {
            Execute();
            return this;
        }
    }
}
