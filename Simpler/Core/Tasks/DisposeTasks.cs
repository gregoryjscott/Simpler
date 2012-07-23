using System;
using System.Collections.Generic;
using System.Linq;

namespace Simpler.Core.Tasks
{
    public class DisposeTasks : Task
    {
        // Inputs
        public virtual Task Owner { get; set; }
        public virtual string[] InjectedTaskNames { get; set; }

        public override void Run()
        {
            var jobNames = new List<string>(InjectedTaskNames);
            var properties = Owner.GetType().GetProperties();

            foreach (var property in properties.Where(
                property => property.PropertyType.IsSubclassOf(typeof(Task))
                    &&
                    jobNames.Contains(property.PropertyType.FullName)
                    &&
                    (property.GetValue(Owner, null) != null)
                    &&
                    (property.PropertyType.GetInterface(typeof(IDisposable).FullName) != null)))
            {
                ((IDisposable) property.GetValue(Owner, null)).Dispose();
                property.SetValue(Owner, null, null);
            }
        }
    }
}
