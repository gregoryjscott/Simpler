using System;
using System.Collections.Generic;

namespace Simpler.Injection.Jobs
{
    /// <summary>
    /// Job that will dispose of any sub-jobs that were injected that implement IDisposable.
    /// </summary>
    public class DisposeSubJobs : Job
    {
        // Inputs
        public virtual Job JobContainingSubJobs { get; set; }
        public virtual string[] InjectedSubJobPropertyNames { get; set; }

        public override void Execute()
        {
            var listOfInjected = new List<string>(InjectedSubJobPropertyNames);

            var properties = JobContainingSubJobs.GetType().GetProperties();
            foreach (var propertyX in properties)
            {
                if (propertyX.PropertyType.IsSubclassOf(typeof(Job))
                    &&
                    listOfInjected.Contains(propertyX.PropertyType.FullName) 
                    && 
                    (propertyX.GetValue(JobContainingSubJobs, null) != null )
                    && 
                    (propertyX.PropertyType.GetInterface(typeof(IDisposable).FullName) != null))
                {
                    // Dispose the sub-job.
                    ((IDisposable)propertyX.GetValue(JobContainingSubJobs, null)).Dispose();

                    // Set the sub-job to null. 
                    propertyX.SetValue(JobContainingSubJobs, null, null);
                }
            }
        }
    }
}
