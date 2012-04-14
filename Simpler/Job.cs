using Simpler.Proxy.Jobs;

namespace Simpler
{
    public abstract class Job
    {
        public abstract void Run();

        public static T New<T>()
        {
            var createJob = new CreateJob { JobType = typeof(T) };
            createJob.Run();
            return (T)createJob.JobInstance;
        }
    }
}