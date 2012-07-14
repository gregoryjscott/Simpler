using System;
using Simpler.Core;

namespace Simpler
{
    [InjectJobs]
    public abstract class InJob<TIn> : Job 
    {
        TIn _in;
        public virtual TIn In
        {
            get
            {
                if ((!typeof(TIn).IsValueType) && (_in == null))
                {
                    _in = (TIn)Activator.CreateInstance(typeof(TIn));
                }

                return _in;
            }
            set { _in = value; }
        }
    }
}