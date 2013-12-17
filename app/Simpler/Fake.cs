using System;
using Simpler.Core;
using Simpler.Core.Tasks;

namespace Simpler
{
    public class Fake
    {
        public static TTask Task<TTask>() where TTask : T
        {
            return Task<TTask>(execute => {});
        }

        public static TTask Task<TTask>(Action<TTask> execute) where TTask : T
        {
            var interceptor = new ExecuteInterceptor(
                invocation =>
                    {
                        var executeTask = Simpler.T.New<ExecuteTask>();
                        executeTask.In.Task = (T)invocation.InvocationTarget;
                        executeTask.In.Invocation = new FakeInvocation<TTask>((T)invocation.InvocationTarget, execute);
                        executeTask.Execute();
                    });

            var createTask = Simpler.T.New<CreateTask>();
            createTask.In.TaskType = typeof(TTask);
            createTask.In.ExecuteInterceptor = interceptor;
            createTask.Execute();

            return (TTask)createTask.Out.TaskInstance;
        }
    }
}