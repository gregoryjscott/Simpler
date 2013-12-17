using System.Collections.Generic;

namespace Simpler.Core.Tasks
{
    public class InjectTasks : IO<InjectTasks.Ins, InjectTasks.Outs>
    {
        public class Ins
        {
            public T TaskContainingSubTasks { get; set; }
        }

        public class Outs
        {
            public string[] InjectedSubTaskPropertyNames { get; set; }
        }

        public CreateTask CreateTask { get; set; }

        public override void Execute()
        {
            // This InjectTasks task can't have it's subtasks injected, because it would need to call InjectTasks...I'm getting dizzy.
            if (CreateTask == null) CreateTask = new CreateTask();

            var listOfInjected = new List<string>();

            var properties = In.TaskContainingSubTasks.GetType().GetProperties();
            foreach (var propertyX in properties)
            {
                if (propertyX.PropertyType.IsSubclassOf(typeof(T))
                    && 
                    (propertyX.CanWrite && propertyX.GetValue(In.TaskContainingSubTasks, null) == null))
                {
                    CreateTask.In.TaskType = propertyX.PropertyType;
                    CreateTask.Execute();

                    propertyX.SetValue(In.TaskContainingSubTasks, CreateTask.Out.TaskInstance, null);

                    listOfInjected.Add(propertyX.PropertyType.FullName);
                }
            }

            Out.InjectedSubTaskPropertyNames = listOfInjected.ToArray();
        }
    }
}
