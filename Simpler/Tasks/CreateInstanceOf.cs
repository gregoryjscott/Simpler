using Castle.DynamicProxy;
using Simpler.Attributes;
using System;
using Simpler.Interceptors;

namespace Simpler.Tasks
{
    public class CreateInstanceOf<T> : Task where T : Task
    {
        // Outputs
        public virtual T TaskInstance { get; private set; }

        public override void Execute()
        {
            if (Attribute.IsDefined(typeof(T), typeof(ExecutionCallbacksAttribute)))
            {
                TaskInstance = new ProxyGenerator().CreateClassProxy<T>(new TaskExecutionInterceptor<T>());
            }
            else
            {
                TaskInstance = Activator.CreateInstance<T>();
            }
        }
    }
}
