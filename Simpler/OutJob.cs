using Simpler.Proxy;

namespace Simpler
{
    [InjectJobs]
    public abstract class OutJob<TOut> : Job
    {
        public TOut _Out { get; protected set; }

        public TOut Get()
        {
            Run();
            return _Out;
        }
    }
}