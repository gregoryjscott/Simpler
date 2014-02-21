using System;
using System.Linq;
using System.Reflection;

namespace Simpler.Core.Tasks
{
    public class DisposeTasks: InTask<DisposeTasks.Input>
    {
        public class Input
        {
            public Task Owner { get; set; }
            public string[] InjectedTaskNames { get; set; }
        }

        public override void Execute()
        {
            var taskProperties = In.Owner.GetType().GetProperties();
            var disposableProperties = taskProperties.Where(p => IsSubTask(p) && WasInjected(p) && HasValue(p) && IsDisposable(p));

            foreach (var property in disposableProperties)
            {
                ((IDisposable)property.GetValue(In.Owner, null)).Dispose();
                property.SetValue(In.Owner, null, null);
            }
        }

        #region Helpers

        static bool IsDisposable(PropertyInfo property) { return property.PropertyType.GetInterface(typeof (IDisposable).FullName) != null; }

        bool WasInjected(PropertyInfo property) { return In.InjectedTaskNames.Contains(property.PropertyType.FullName); }

        bool HasValue(PropertyInfo property) { return property.GetValue(In.Owner, null) != null; }

        #endregion
    }
}
