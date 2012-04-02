using System;
using Simpler.Construction.Tasks;

namespace Simpler
{
    public class Test<TTask> where TTask : Task
    {
        TTask Task { get; set; }

        public static Test<TTask> Create()
        {
            var createTask = new CreateTask { TaskType = typeof(TTask) };
            createTask.Execute();

            var task = (TTask)createTask.TaskInstance;
            var test = new Test<TTask> { Task = task };

            return test;
        }

        public Test<TTask> Arrange(Action<TTask> arrange)
        {
            arrange(Task);
            return this;
        }

        public Test<TTask> Act()
        {
            Task.Execute();
            return this;
        }

        public void Assert(Action<TTask> assert)
        {
            assert(Task);
        }
    }
}