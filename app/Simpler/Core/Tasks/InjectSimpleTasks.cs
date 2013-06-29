using System.Collections.Generic;

namespace Simpler.Core.Tasks
{
    public class InjectSimpleTasks : InOutSimpleTask<InjectSimpleTasks.Input, InjectSimpleTasks.Output>
    {
        public class Input
        {
            public SimpleTask SimpleTaskContainingSubSimpleTasks { get; set; }
        }

        public class Output
        {
            public string[] InjectedSubTaskPropertyNames { get; set; }
        }

        public CreateSimpleTask CreateSimpleTask { get; set; }

        public override void Execute()
        {
            // This InjectSimpleTasks SimpleTask can't have it's subtasks injected, because it would need to call InjectSimpleTasks...I'm getting dizzy.
            if (CreateSimpleTask == null) CreateSimpleTask = new CreateSimpleTask();

            var listOfInjected = new List<string>();

            var properties = In.SimpleTaskContainingSubSimpleTasks.GetType().GetProperties();
            foreach (var propertyX in properties)
            {
                if (propertyX.PropertyType.IsSubclassOf(typeof(SimpleTask))
                    && 
                    (propertyX.CanWrite && propertyX.GetValue(In.SimpleTaskContainingSubSimpleTasks, null) == null))
                {
                    CreateSimpleTask.In.TaskType = propertyX.PropertyType;
                    CreateSimpleTask.Execute();

                    propertyX.SetValue(In.SimpleTaskContainingSubSimpleTasks, CreateSimpleTask.Out.TaskInstance, null);

                    listOfInjected.Add(propertyX.PropertyType.FullName);
                }
            }

            Out.InjectedSubTaskPropertyNames = listOfInjected.ToArray();
        }
    }
}
