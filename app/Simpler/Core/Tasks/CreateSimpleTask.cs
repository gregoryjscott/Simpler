using Castle.DynamicProxy;
using System;

namespace Simpler.Core.Tasks
{
    public class CreateSimpleTask : InOutSimpleTask<CreateSimpleTask.Input, CreateSimpleTask.Output>
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

        public ExecuteSimpleTask ExecuteSimpleTask { get; set; }

        public override void Execute()
        {
            if (In.ExecuteInterceptor == null)
            {
                In.ExecuteInterceptor = new ExecuteInterceptor(
                    invocation =>
                        {
                            if (ExecuteSimpleTask == null) ExecuteSimpleTask = new ExecuteSimpleTask();
                            ExecuteSimpleTask.In.SimpleTask = (SimpleTask)invocation.InvocationTarget;
                            ExecuteSimpleTask.In.Invocation = invocation;
                            ExecuteSimpleTask.Execute();
                        });
            }

            Out.TaskInstance = ProxyGenerator.CreateClassProxy(In.TaskType, In.ExecuteInterceptor);
        }
    }
}
