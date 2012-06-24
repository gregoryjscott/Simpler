using System;
using Simpler.Proxy;

namespace Simpler
{
    [InjectJobs]
    public abstract class InJob<TIn> : Job 
        where TIn : class
    {
        TIn _in;
        public virtual TIn In
        {
            get { return _in ?? (_in = (TIn) Activator.CreateInstance(typeof (TIn))); }
            set { _in = value; }
        }
    }
}