using System;
using System.Collections.Generic;

namespace Simpler.Injection.Tasks
{
    /// <summary>
    /// Task that will dispose of any sub-tasks that were injected that implement IDisposable.
    /// </summary>
    public class DisposeSubTasks : Task
    {
        // Inputs
        public virtual Task TaskContainingSubTasks { get; set; }
        public virtual string[] InjectedSubTaskPropertyNames { get; set; }

        public override void Execute()
        {
            var listOfInjected = new List<string>(InjectedSubTaskPropertyNames);

            var properties = TaskContainingSubTasks.GetType().GetProperties();
            foreach (var propertyX in properties)
            {
                if (propertyX.PropertyType.IsSubclassOf(typeof(Task))
                    &&
                    listOfInjected.Contains(propertyX.PropertyType.FullName) 
                    && 
                    (propertyX.GetValue(TaskContainingSubTasks, null) != null )
                    && 
                    (propertyX.PropertyType.GetInterface(typeof(IDisposable).FullName) != null))
                {
                    // Dispose the sub-task.
                    ((IDisposable)propertyX.GetValue(TaskContainingSubTasks, null)).Dispose();

                    // Set the sub-task to null. 
                    propertyX.SetValue(TaskContainingSubTasks, null, null);
                }
            }
        }
    }
}
