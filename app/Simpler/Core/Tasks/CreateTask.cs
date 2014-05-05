using System;

namespace Simpler.Core.Tasks
{
    public class CreateTask : InOutTask<CreateTask.Input, CreateTask.Output>
    {
        public class Input
        {
            public Type TaskType { get; set; }
            public Action<Task> ExecuteOverride { get; set; }
        }

        public class Output
        {
            public object TaskInstance { get; set; }
        }

        public override void Execute()
        {
            var instance = Activator.CreateInstance(In.TaskType);
            Out.TaskInstance = new TaskProxy(In.TaskType, instance, In.ExecuteOverride).GetTransparentProxy();
        }
    }
}
