using System;
using System.Collections.Generic;
using System.Reflection;

namespace Simpler.Injection.Tasks
{
    public class DisposeSubTasks : Task
    {
        // Inputs
        public virtual Task TaskContainingSubTasks { get; set; }
        public virtual string[] InjectedSubTaskPropertyNames { get; set; }

        public override void Execute()
        {
            var listOfInjected = new List<string>(InjectedSubTaskPropertyNames);

            PropertyInfo[] properties = TaskContainingSubTasks.GetType().GetProperties();
            foreach (PropertyInfo propertyX in properties)
            {
                if (propertyX.PropertyType.IsSubclassOf(typeof(Task)))
                {
                    if (listOfInjected.Contains(propertyX.PropertyType.FullName) 
                        && 
                        propertyX.GetValue(TaskContainingSubTasks, null) != null 
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
}
