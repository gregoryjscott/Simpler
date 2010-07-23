using System.Reflection;
using System.Collections.Generic;
using Simpler.Construction.Tasks;

namespace Simpler.Injection.Tasks
{
    public class InjectSubTasks : Task
    {
        // Inputs
        public virtual Task TaskContainingSubTasks { get; set; }

        // Outputs
        public virtual string[] InjectedSubTaskPropertyNames { get; private set; }

        // Sub-tasks
        public CreateTask CreateTask { get; set; }

        public override void Execute()
        {
            if (CreateTask == null) CreateTask = new CreateTask();

            var listOfInjected = new List<string>();

            PropertyInfo[] properties = TaskContainingSubTasks.GetType().GetProperties();
            foreach (PropertyInfo propertyX in properties)
            {
                if (propertyX.PropertyType.IsSubclassOf(typeof(Task)))
                {
                    if (propertyX.CanWrite && propertyX.GetValue(TaskContainingSubTasks, null) == null)
                    {
                        CreateTask.TaskType = propertyX.PropertyType;
                        CreateTask.Execute();
                        propertyX.SetValue(TaskContainingSubTasks, CreateTask.TaskInstance, null);

                        listOfInjected.Add(propertyX.PropertyType.FullName);
                    }
                }
            }

            InjectedSubTaskPropertyNames = listOfInjected.ToArray();
        }
    }
}
