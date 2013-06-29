using System;

namespace Simpler
{
    public abstract class InOutSimpleTask<TIn, TOut> : SimpleTask
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

        TOut _out;
        public virtual TOut Out
        {
            get
            {
                if ((!typeof(TOut).IsValueType) && (_out == null))
                {
                    _out = (TOut)Activator.CreateInstance(typeof(TOut));
                }

                return _out;
            }
            set { _out = value; }
        }
    }
}