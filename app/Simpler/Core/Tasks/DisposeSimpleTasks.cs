using System;
using System.Collections.Generic;
using System.Linq;

namespace Simpler.Core.Tasks
{
    public class DisposeSimpleTasks : InSimpleTask<DisposeSimpleTasks.Input>
    {
        public class Input
        {
            public SimpleTask Owner { get; set; }
            public string[] InjectedTaskNames { get; set; }
        }

        public override void Execute()
        {
            var taskNames = new List<string>(In.InjectedTaskNames);
            var properties = In.Owner.GetType().GetProperties();

            foreach (var property in properties.Where(
                property => property.PropertyType.IsSubclassOf(typeof(SimpleTask))
                    &&
                    taskNames.Contains(property.PropertyType.FullName)
                    &&
                    (property.GetValue(In.Owner, null) != null)
                    &&
                    (property.PropertyType.GetInterface(typeof(IDisposable).FullName) != null)))
            {
                ((IDisposable) property.GetValue(In.Owner, null)).Dispose();
                property.SetValue(In.Owner, null, null);
            }
        }
    }
}
