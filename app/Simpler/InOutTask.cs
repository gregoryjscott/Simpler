using System;
using Newtonsoft.Json;
using Simpler.Core.Interfaces;

namespace Simpler
{
    public abstract class InOutTask<TIn, TOut> : Task, IInTask<TIn>, IOutTask<TOut>
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

        public virtual string InJson()
        {
            return JsonConvert.SerializeObject(In);
        }

        public virtual string OutJson()
        {
            return JsonConvert.SerializeObject(Out);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(new { Name, In});
        }
    }
}