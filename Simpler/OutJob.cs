using System;
using Simpler.Core;

namespace Simpler
{
    [InjectJobs]
    public abstract class OutJob<TOut> : Job
        where TOut : class
    {
        TOut _out;
        public TOut Out
        {
            get { return _out ?? (_out = (TOut)Activator.CreateInstance(typeof(TOut))); }
            set { _out = value; }
        }
    }
}