using System.Collections.Generic;

namespace Simpler.Core.Tasks
{
    public class InjectTasks : InOutTask<InjectTasks.Input, InjectTasks.Output>
    {
        public class Input
        {
            public Task TaskContainingSubTasks { get; set; }
        }

        public class Output
        {
            public string[] InjectedSubTaskPropertyNames { get; set; }
        }

        public CreateTask CreateTask { get; set; }

        public override void Execute()
        {
            if (CreateTask == null) CreateTask = new CreateTask();

            var listOfInjected = new List<string>();

            var properties = In.TaskContainingSubTasks.GetType().GetProperties();
            foreach (var propertyX in properties)
            {
                if (propertyX.PropertyType.IsSubclassOf(typeof(Task))
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
