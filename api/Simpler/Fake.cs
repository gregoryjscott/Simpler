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
            var createTask = Simpler.Task.New<CreateTask>();
            createTask.In.TaskType = typeof(TTask);
            createTask.In.FakeExecute = t => execute((TTask)t);
            createTask.Execute();

            return (TTask)createTask.Out.TaskInstance;
        }

        public static void SubTasks(Task task)
        {
            var properties = task.GetType().GetProperties();
            foreach (var propertyX in properties)
            {
                if (propertyX.PropertyType.IsSubclassOf(typeof(Task)) && (propertyX.CanWrite))
                {
                    var createTask = Simpler.Task.New<CreateTask>();
                    createTask.In.TaskType = propertyX.PropertyType;
                    createTask.In.FakeExecute = t => {};
                    createTask.Execute();
                    propertyX.SetValue(task, createTask.Out.TaskInstance, null);
                }
            }
        }
    }
}