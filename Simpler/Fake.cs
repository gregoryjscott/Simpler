using System;
using Simpler.Core;
using Simpler.Core.Tasks;

namespace Simpler
{
    public class Fake
    {
        public static TTask Task<TTask>()
        {
            var createTask = Simpler.Task.New<CreateTask>();
            createTask.In.TaskType = typeof(TTask);
            createTask.Execute();

            return (TTask)createTask.Out.TaskInstance;
        }

        public static TTask Task<TTask>(Action<TTask> execute)
        {
            var interceptor = new ExecuteInterceptor(invocation => execute((TTask)invocation.InvocationTarget));

            var createTask = Simpler.Task.New<CreateTask>();
            createTask.In.TaskType = typeof(TTask);
            createTask.In.ExecuteInterceptor = interceptor;
            createTask.Execute();

            return (TTask)createTask.Out.TaskInstance;
        }
    }
}