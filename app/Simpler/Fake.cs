using System;
using Simpler.Core;
using Simpler.Core.Tasks;

namespace Simpler
{
    public class Fake
    {
        public static TTask Task<TTask>() where TTask : Task
        {
            return Task<TTask>(execute => {});
        }

        public static TTask Task<TTask>(Action<TTask> execute) where TTask : Task
        {
            //var interceptor = new ExecuteInterceptor(
            //    invocation =>
            //        {
            //            var executeTask = Simpler.Task.New<ExecuteTask>();
            //            executeTask.In.Task = (Task)invocation.InvocationTarget;
            //            executeTask.In.Invocation = new FakeInvocation<TTask>((Task)invocation.InvocationTarget, execute);
            //            executeTask.Execute();
            //        });

            var createTask = Simpler.Task.New<CreateTask>();
            createTask.In.TaskType = typeof(TTask);
            createTask.In.ExecuteOverride = t => execute((TTask)t);
            createTask.Execute();

            return (TTask)createTask.Out.TaskInstance;
        }
    }
}