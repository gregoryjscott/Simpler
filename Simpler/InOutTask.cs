using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class InOutTask<TInput, TOutput> : Task
    {
        public TInput Input { get; set; }
        public TOutput Output { get; set; }
    }
}