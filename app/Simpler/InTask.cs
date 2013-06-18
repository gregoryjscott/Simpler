using System;
using Newtonsoft.Json;
using Simpler.Core.Interfaces;

namespace Simpler
{
    public abstract class InTask<TIn> : Task, IInTask<TIn>
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

        public virtual string InJson()
        {
            return JsonConvert.SerializeObject(In);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}