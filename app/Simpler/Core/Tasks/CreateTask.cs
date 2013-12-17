using Castle.DynamicProxy;
using System;

namespace Simpler.Core.Tasks
{
    public class CreateTask : IO<CreateTask.Ins, CreateTask.Outs>
    {
        static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

        public class Ins
        {
            public Type TaskType { get; set; }
            public ExecuteInterceptor ExecuteInterceptor { get; set; }
        }

        public class Outs
        {
            public object TaskInstance { get; set; }
        }

        public ExecuteTask ExecuteTask { get; set; }

        public override void Execute()
        {
            if (In.ExecuteInterceptor == null)
            {
                In.ExecuteInterceptor = new ExecuteInterceptor(
                    invocation =>
                        {
                            if (ExecuteTask == null) ExecuteTask = new ExecuteTask();
                            ExecuteTask.In.Task = (T)invocation.InvocationTarget;
                            ExecuteTask.In.Invocation = invocation;
                            ExecuteTask.Execute();
                        });
            }

            Out.TaskInstance = ProxyGenerator.CreateClassProxy(In.TaskType, In.ExecuteInterceptor);
        }
    }
}
