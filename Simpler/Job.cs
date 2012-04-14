using Simpler.Construction.Jobs;

namespace Simpler
{
    public abstract class Job
    {
        public abstract void Execute();

        public static T Create<T>()
        {
            var createJob = new CreateJob { JobType = typeof(T) };
            createJob.Execute();
            return (T)createJob.JobInstance;
        }
    }
}