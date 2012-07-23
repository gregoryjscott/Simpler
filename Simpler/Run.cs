using System;
using Simpler.Core.Tasks;

namespace Simpler
{
    public static class Run<TTask>
    {
        public class Output
        {
            public Output(dynamic task)
            {
                _task = task;
            }

            dynamic _task;
            public dynamic Get()
            {
                _task.Run();
                return _task.Out;
            }
        }

        public static Output Set(Action<dynamic> set)
        {
            var createTask = new CreateTask {TaskType = typeof (TTask)};
            createTask.Run();
            dynamic task = createTask.TaskInstance;

            set(task.In);

            return new Output(task);
        }

        public static dynamic Get()
        {
            var createTask = new CreateTask { TaskType = typeof(TTask) };
            createTask.Run();
            dynamic task = createTask.TaskInstance;

            task.Run();
            return task.Out;
        }
    }
}
