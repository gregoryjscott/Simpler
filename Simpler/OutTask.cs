using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class OutTask<TOutputs> : Task
    {
        protected TOutputs Out { get; set; }

        public virtual TOutputs GetOutputs()
        {
            Execute();
            return Out;
        }
    }
}