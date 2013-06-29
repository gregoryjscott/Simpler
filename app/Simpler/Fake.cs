using System;
using Simpler.Core;
using Simpler.Core.Tasks;

namespace Simpler
{
    public class Fake
    {
        public static TTask Task<TTask>() where TTask : SimpleTask
        {
            return Task<TTask>(execute => {});
        }

        public static TTask Task<TTask>(Action<TTask> execute) where TTask : SimpleTask
        {
            var interceptor = new ExecuteInterceptor(
                invocation =>
                    {
                        var executeTask = Simpler.SimpleTask.New<ExecuteSimpleTask>();
                        executeTask.In.SimpleTask = (SimpleTask)invocation.InvocationTarget;
                        executeTask.In.Invocation = new FakeInvocation<TTask>((SimpleTask)invocation.InvocationTarget, execute);
                        executeTask.Execute();
                    });

            var createTask = Simpler.SimpleTask.New<CreateSimpleTask>();
            createTask.In.TaskType = typeof(TTask);
            createTask.In.ExecuteInterceptor = interceptor;
            createTask.Execute();

            return (TTask)createTask.Out.TaskInstance;
        }
    }
}