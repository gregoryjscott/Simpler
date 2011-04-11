using Castle.DynamicProxy;
using System;
using Simpler.Construction.Interceptors;

namespace Simpler.Construction.Tasks
{
    /// <summary>
    /// Task used to instantiate tasks.  If the given task type is decorated with ExecutionCallbacksAttribute
    /// then a proxy class is created so that task execution events can be fired.
    /// </summary>
    public class CreateTask : Task
    {
        /// <summary>
        /// This class provided by Castle that allows for creating proxy classes.  Creating it is expensive - we
        /// only want to do it once.
        /// </summary>
        static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

        // Inputs
        public virtual Type TaskType { get; set; }

        // Outputs
        public virtual object TaskInstance { get; private set; }

        public override void Execute()
        {
            if (Attribute.IsDefined(TaskType, typeof(ExecutionCallbacksAttribute)))
            {
                TaskInstance = ProxyGenerator.CreateClassProxy(TaskType, new TaskExecutionInterceptor());
            }
            else
            {
                TaskInstance = Activator.CreateInstance(TaskType);
            }
        }
    }
}
