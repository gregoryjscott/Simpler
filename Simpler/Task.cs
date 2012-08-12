using System;
using Simpler.Core.Tasks;

namespace Simpler
{
    public abstract class Task
    {
        public virtual void Specs() { throw new NoSpecsException(); }

        public static T New<T>()
        {
            var createTask = new CreateTask { TaskType = typeof(T) };
            createTask.Execute();
            return (T)createTask.TaskInstance;
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

        public abstract void Execute();
    }
}