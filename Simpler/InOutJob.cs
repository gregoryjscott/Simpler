using Simpler.Proxy;

namespace Simpler
{
    [_InjectJobs]
    public abstract class InOutJob<TIn, TOut> : Job
    {
        public TIn _In { get; set; }
        public TOut _Out { get; set; }

        public virtual InOutJob<TIn, TOut> Set(TIn _in)
        {
            _In = _in;
            return this;
        }

        public virtual TOut Get()
        {
            Run();
            return _Out;
        }
    }
}