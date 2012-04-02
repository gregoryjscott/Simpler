using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class InOutTask<TInput, TOutput> : Task
    {
        public TInput Input { protected get; set; }
        public TOutput Output { get; protected set; }

        public InOutTask<TInput, TOutput> Set(TInput input)
        {
            Input = input;
            return this;
        }

        public TOutput Get()
        {
            Execute();
            return Output;
        }
    }
}