using System;
using Newtonsoft.Json;
using Simpler.Core.Interfaces;

namespace Simpler
{
    public abstract class OutTask<TOut> : Task, IOutTask<TOut>
    {
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

        public virtual string OutJson()
        {
            return JsonConvert.SerializeObject(Out);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}