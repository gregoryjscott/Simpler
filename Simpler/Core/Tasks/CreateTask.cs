using Castle.DynamicProxy;
using System;

namespace Simpler.Core.Tasks
{
    public class CreateTask : Task
    {
        static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

        // Inputs
        public virtual Type TaskType { get; set; }
        public RunInterceptor RunInterceptor { get; set; }

        // Outputs
        public virtual object TaskInstance { get; private set; }

        public FireEvents FireEvents { get; set; }

        public override void Run()
        {
            if (RunInterceptor == null)
            {
                RunInterceptor = new RunInterceptor(
                    invocation =>
                        {
                            if (FireEvents == null) FireEvents = new FireEvents();
                            FireEvents.Task = (Task)invocation.InvocationTarget;
                            FireEvents.Invocation = invocation;
                            FireEvents.Run();
                        });
            }

            TaskInstance = ProxyGenerator.CreateClassProxy(TaskType, RunInterceptor);
        }
    }
}
