using Castle.DynamicProxy;
using System;

namespace Simpler.Core.Tasks
{
    public class CreateTask : Task
    {
        static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

        // Inputs
        public virtual Type TaskType { get; set; }
        public ExecuteInterceptor ExecuteInterceptor { get; set; }

        // Outputs
        public virtual object TaskInstance { get; private set; }

        public FireEvents FireEvents { get; set; }

        public override void Execute()
        {
            if (ExecuteInterceptor == null)
            {
                ExecuteInterceptor = new ExecuteInterceptor(
                    invocation =>
                        {
                            if (FireEvents == null) FireEvents = new FireEvents();
                            FireEvents.Task = (Task)invocation.InvocationTarget;
                            FireEvents.Invocation = invocation;
                            FireEvents.Execute();
                        });
            }

            TaskInstance = ProxyGenerator.CreateClassProxy(TaskType, ExecuteInterceptor);
        }
    }
}
