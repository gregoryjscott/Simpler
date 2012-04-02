using System.Collections.Generic;
using Simpler.Construction.Tasks;

namespace Simpler.Injection.Tasks
{
    /// <summary>
    /// Task that will search a task for sub-tasks and create them if they are null.
    /// </summary>
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

            var properties = TaskContainingSubTasks.GetType().GetProperties();
            foreach (var propertyX in properties)
            {
                if (propertyX.PropertyType.IsSubclassOf(typeof(Task))
                    && 
                    (propertyX.CanWrite && propertyX.GetValue(TaskContainingSubTasks, null) == null))
                {
                    CreateTask.TaskType = propertyX.PropertyType;
                    CreateTask.Execute();

                    propertyX.SetValue(TaskContainingSubTasks, CreateTask.TaskInstance, null);

                    listOfInjected.Add(propertyX.PropertyType.FullName);
                }
            }

            InjectedSubTaskPropertyNames = listOfInjected.ToArray();
        }
    }
}
