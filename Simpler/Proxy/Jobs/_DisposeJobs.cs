using System;
using System.Collections.Generic;
using System.Linq;

namespace Simpler.Proxy.Jobs
{
    public class _DisposeJobs : Job
    {
        // Inputs
        public virtual Job Owner { get; set; }
        public virtual string[] InjectedJobNames { get; set; }

        public override void Run()
        {
            var jobNames = new List<string>(InjectedJobNames);
            var properties = Owner.GetType().GetProperties();

            foreach (var property in properties.Where(
                property => property.PropertyType.IsSubclassOf(typeof(Job))
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
