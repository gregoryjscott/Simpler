using Simpler.Injection;

namespace Simpler
{
    [InjectSubTasks]
    public abstract class InTask<TInput> : Task
    {
        public TInput Input { protected get; set; }

        public InTask<TInput> Set(TInput input)
        {
            Input = input;
            return this;
        }
    }
}