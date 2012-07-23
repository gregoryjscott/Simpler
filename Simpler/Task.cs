using System;
using Simpler.Core.Jobs;

namespace Simpler
{
    public abstract class Task
    {
        public virtual void Specs() { throw new NoSpecsException(); }

        public static T New<T>()
        {
            var createJob = new CreateTask { JobType = typeof(T) };
            createJob.Run();
            return (T)createJob.JobInstance;
        }

        public virtual string Name
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
    }
}