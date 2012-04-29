using System;
using Simpler.Proxy.Jobs;

namespace Simpler
{
    public abstract class Job
    {
        public static T New<T>()
        {
            var createJob = new _CreateJob { JobType = typeof(T) };
            createJob.Run();
            return (T)createJob.JobInstance;
        }

        public string Name
        {
            get
            {
                var baseType = GetType().BaseType;
                return baseType == null
                           ? "Unknown"
                           : String.Format("{0}.{1}", baseType.Namespace, baseType.Name);
            }
        }

        public abstract void Run();
        
        public virtual void Test() { throw new SimplerException("No tests."); }

        public virtual void Check(bool condition, string errorMessage)
        {
            if (!condition) throw new SimplerException(errorMessage);
        }
    }
}