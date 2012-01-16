using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class OutTask<TOutputs> : Task
    {
        public virtual TOutputs Outputs { get; set; }

        public virtual TOutputs GetOutputs()
        {
            Execute();
            return Outputs;
        }
    }
}
