using System;

namespace Simpler
{
    public abstract class OutSimpleTask<TOut> : SimpleTask
    {
        TOut _out;
        public TOut Out
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