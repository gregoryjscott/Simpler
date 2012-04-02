using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class InTask<TInput> : Task
    {
        public TInput Input { get; set; }
    }
}