using System.Collections.Generic;

namespace Simpler.Core.Tasks
{
    public class InjectTasks : IO<InjectTasks.Ins, InjectTasks.Outs>
    {
        public class Ins
        {
            public T Task { get; set; }
        }

        public class Outs
        {
            public string[] InjectedTaskPropertyNames { get; set; }
        }

        public CreateTask CreateTask { get; set; }

        public override void Execute()
        {
            if (CreateTask == null) CreateTask = new CreateTask();

            var injected = new List<string>();

            var properties = In.Task.GetType().GetProperties();
            foreach (var propertyX in properties)
            {
                if (propertyX.PropertyType.IsSubclassOf(typeof(T))
                    && 
                    (propertyX.CanWrite && propertyX.GetValue(In.Task, null) == null))
                {
                    CreateTask.In.TaskType = propertyX.PropertyType;
                    CreateTask.Execute();

                    propertyX.SetValue(In.Task, CreateTask.Out.TaskInstance, null);

                    injected.Add(propertyX.PropertyType.FullName);
                }
            }

            Out.InjectedTaskPropertyNames = injected.ToArray();
        }
    }
}
