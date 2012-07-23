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
            createTask.TaskType = typeof(TTask);
            createTask.Run();

            return (TTask)createTask.TaskInstance;
        }

        public static TTask Task<TTask>(Action<TTask> run)
        {
            var interceptor = new RunInterceptor(invocation => run((TTask)invocation.InvocationTarget));

            var createTask = Simpler.Task.New<CreateTask>();
            createTask.TaskType = typeof(TTask);
            createTask.RunInterceptor = interceptor;
            createTask.Run();

            return (TTask)createTask.TaskInstance;
        }
    }
}