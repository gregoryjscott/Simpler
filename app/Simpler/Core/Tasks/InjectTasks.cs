using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Simpler.Core.Tasks
{
    public class InjectTasks: InOutTask<InjectTasks.Input, InjectTasks.Output>
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

            var injected = new List<string>();
            var taskProperties = In.TaskContainingSubTasks.GetType().GetProperties();
            var nullProperties = taskProperties.Where(property => IsSubTask(property) && property.CanWrite && HasNoValue(property));

            foreach (var property in nullProperties)
            {
                CreateTask.In.TaskType = property.PropertyType;
                CreateTask.Execute();
                var task = CreateTask.Out.TaskInstance;

                property.SetValue(In.TaskContainingSubTasks, task, null);
                injected.Add(property.PropertyType.FullName);
            }

            Out.InjectedSubTaskPropertyNames = injected.ToArray();
        }

        #region Helpers

        bool HasNoValue(PropertyInfo property) { return property.GetValue(In.TaskContainingSubTasks, null) == null; }

        #endregion
    }
}
