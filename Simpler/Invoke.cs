using System;
using Simpler.Construction.Tasks;

namespace Simpler
{
    public class Invoke<TTask> : Task where TTask : Task
    {
        // TODO - this can be made private after all the tests are changed to use Test<TTask> instead of Invoke<TTask>.New().Task
        public TTask Task { get; set; }

        public static Invoke<TTask> New()
        {
            var invoke = new Invoke<TTask>();
            invoke.CreateTaskIfNull();
            return invoke;
        }

        public Invoke<TTask> Set(Action<TTask> set)
        {
            CreateTaskIfNull();
            set(Task);
            return this;
        }

        public TTask Get()
        {
            CreateTaskIfNull();
            Execute();
            return Task;
        }

        public override void Execute()
        {
            CreateTaskIfNull();
            Task.Execute();
        }

        void CreateTaskIfNull()
        {
            if (Task != null) return;

            var createTask = new CreateTask { TaskType = typeof(TTask) };
            createTask.Execute();
            Task = (TTask)createTask.TaskInstance;
        }
    }
}