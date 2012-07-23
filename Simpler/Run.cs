using System;
using Simpler.Core.Tasks;

namespace Simpler
{
    public static class Run<TTask>
    {
        public class Output
        {
            public Output(dynamic job)
            {
                _job = job;
            }

            dynamic _job;
            public dynamic Get()
            {
                _job.Run();
                return _job.Out;
            }
        }

        public static Output Set(Action<dynamic> set)
        {
            var createTask = new CreateTask {TaskType = typeof (TTask)};
            createTask.Run();
            dynamic job = createTask.TaskInstance;

            set(job.In);

            return new Output(job);
        }

        public static dynamic Get()
        {
            var createTask = new CreateTask { TaskType = typeof(TTask) };
            createTask.Run();
            dynamic job = createTask.TaskInstance;

            job.Run();
            return job.Out;
        }
    }
}
