using Simpler.Injection;

namespace Simpler
{
    [InjectSubJobs]
    public abstract class InJob<TInput> : Job
    {
        public TInput Input { protected get; set; }

        public InJob<TInput> Set(TInput input)
        {
            Input = input;
            return this;
        }
    }
}