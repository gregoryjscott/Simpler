using System;
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
            var fakeTask = Simpler.Task.New<FakeTask>();
            fakeTask.In.TaskType = typeof(TTask);
            fakeTask.In.ExecuteOverride = t => execute((TTask)t);
            fakeTask.Execute();

            return (TTask)fakeTask.Out.TaskInstance;
        }

        public static void SubTasks(Task task)
        {
            var properties = task.GetType().GetProperties();
            foreach (var propertyX in properties)
            {
                if (propertyX.PropertyType.IsSubclassOf(typeof(Task)) && (propertyX.CanWrite))
                {
                    var fakeTask = Simpler.Task.New<FakeTask>();
                    fakeTask.In.TaskType = propertyX.PropertyType;
                    fakeTask.In.ExecuteOverride = t => {};
                    fakeTask.Execute();
                    propertyX.SetValue(task, fakeTask.Out.TaskInstance, null);
                }
            }
        }
    }
}