using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class OutTask<TOutput> : Task
    {
        public TOutput Output { get; protected set; }

        public OutTask<TOutput> Get()
        {
            Execute();
            return this;
        }
    }
}