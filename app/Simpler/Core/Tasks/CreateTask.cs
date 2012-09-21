using Castle.DynamicProxy;
using System;

namespace Simpler.Core.Tasks
{
    public class CreateTask : InOutTask<CreateTask.Input, CreateTask.Output>
    {
        static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

        public class Input
        {
            public Type TaskType { get; set; }
            public ExecuteInterceptor ExecuteInterceptor { get; set; }
        }

        public class Output
        {
            public object TaskInstance { get; set; }
        }

        public FireEvents FireEvents { get; set; }

        public override void Execute()
        {
            if (In.ExecuteInterceptor == null)
            {
                In.ExecuteInterceptor = new ExecuteInterceptor(
                    invocation =>
                        {
                            if (FireEvents == null) FireEvents = new FireEvents();
                            FireEvents.In.Task = (Task)invocation.InvocationTarget;
                            FireEvents.In.Invocation = invocation;
                            FireEvents.Execute();
                        });
            }

            Out.TaskInstance = ProxyGenerator.CreateClassProxy(In.TaskType, In.ExecuteInterceptor);
        }
    }
}
