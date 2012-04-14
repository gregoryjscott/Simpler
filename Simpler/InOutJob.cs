using Simpler.Injection;

namespace Simpler
{
    [InjectSubJobs]
    public abstract class InOutJob<TInput, TOutput> : Job
    {
        public TInput Input { protected get; set; }
        public TOutput Output { get; protected set; }

        public InOutJob<TInput, TOutput> Set(TInput input)
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