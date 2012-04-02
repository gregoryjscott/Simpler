using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class OutTask<TOutput> : Task
    {
        public TOutput Output { get; protected set; }

        public TOutput Get()
        {
            Execute();
            return Output;
        }
    }
}