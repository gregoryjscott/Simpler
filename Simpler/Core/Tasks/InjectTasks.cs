using System.Collections.Generic;

namespace Simpler.Core.Tasks
{
    public class InjectTasks : Task
    {
        // Inputs
        public virtual Task TaskContainingSubTasks { get; set; }

        // Outputs
        public virtual string[] InjectedSubJobPropertyNames { get; private set; }

        // Sub-jobs
        public CreateTask CreateTask { get; set; }

        public override void Run()
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
                    CreateTask.JobType = propertyX.PropertyType;
                    CreateTask.Run();

                    propertyX.SetValue(TaskContainingSubTasks, CreateTask.JobInstance, null);

                    listOfInjected.Add(propertyX.PropertyType.FullName);
                }
            }

            InjectedSubJobPropertyNames = listOfInjected.ToArray();
        }
    }
}
