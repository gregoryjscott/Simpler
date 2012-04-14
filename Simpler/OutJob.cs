using Simpler.Injection;

namespace Simpler
{
    [InjectSubJobs]
    public abstract class OutJob<TOutput> : Job
    {
        public TOutput Output { get; protected set; }

        public TOutput Get()
        {
            Execute();
            return Output;
        }
    }
}