using Castle.DynamicProxy;
using System;

namespace Simpler.Core.Tasks
{
    public class CreateTask : InOutTask<CreateTask.Input, CreateTask.Output>
    {
        //static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

        public class Input
        {
            public Type TaskType { get; set; }
            //public ExecuteInterceptor ExecuteInterceptor { get; set; }
            public Action<Task> ExecuteOverride { get; set; }
        }

        public class Output
        {
            public object TaskInstance { get; set; }
        }

        public ExecuteTask ExecuteTask { get; set; }

        public override void Execute()
        {
            //if (In.ExecuteInterceptor == null)
            //{
            //    In.ExecuteInterceptor = new ExecuteInterceptor(
            //        invocation =>
            //            {
            //                if (ExecuteTask == null) ExecuteTask = new ExecuteTask();
            //                ExecuteTask.In.Task = (Task)invocation.InvocationTarget;
            //                ExecuteTask.In.Invocation = invocation;
            //                ExecuteTask.Execute();
            //            });
            //}

            //Out.TaskInstance = ProxyGenerator.CreateClassProxy(In.TaskType, In.ExecuteInterceptor);
            var instance = Activator.CreateInstance(In.TaskType);
            Out.TaskInstance = new TaskProxy(In.TaskType, instance, In.ExecuteOverride).GetTransparentProxy();
        }

    }
}
