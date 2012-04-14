using Simpler.Proxy;

namespace Simpler
{
    [InjectJobs]
    public abstract class InJob<TIn> : Job
    {
        public TIn _In { protected get; set; }

        public InJob<TIn> Set(TIn _in)
        {
            _In = _in;
            return this;
        }
    }
}