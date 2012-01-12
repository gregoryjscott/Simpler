using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class OutTask<TOutputs> : Task
    {
        public new virtual TOutputs Outputs { get; set; }

        public virtual dynamic GetOutputs()
        {
            return Outputs;
        }

        public OutTask<TOutputs> DoIt()
        {
            Execute();
            return this;
        }
    }
}
