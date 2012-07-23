using System.Collections.Generic;

namespace Simpler.Core.Jobs
{
    public class InjectJobs : Job
    {
        // Inputs
        public virtual Job JobContainingSubJobs { get; set; }

        // Outputs
        public virtual string[] InjectedSubJobPropertyNames { get; private set; }

        // Sub-jobs
        public CreateJob CreateJob { get; set; }

        public override void Run()
        {
            if (CreateJob == null) CreateJob = new CreateJob();

            var listOfInjected = new List<string>();

            var properties = JobContainingSubJobs.GetType().GetProperties();
            foreach (var propertyX in properties)
            {
                if (propertyX.PropertyType.IsSubclassOf(typeof(Job))
                    && 
                    (propertyX.CanWrite && propertyX.GetValue(JobContainingSubJobs, null) == null))
                {
                    CreateJob.JobType = propertyX.PropertyType;
                    CreateJob.Run();

                    propertyX.SetValue(JobContainingSubJobs, CreateJob.JobInstance, null);

                    listOfInjected.Add(propertyX.PropertyType.FullName);
                }
            }

            InjectedSubJobPropertyNames = listOfInjected.ToArray();
        }
    }
}
