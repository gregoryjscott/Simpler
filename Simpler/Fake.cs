using System;
using Simpler.Core;
using Simpler.Core.Tasks;

namespace Simpler
{
    public class Fake
    {
        public static TJob Job<TJob>()
        {
            var createJob = Simpler.Task.New<CreateTask>();
            createJob.JobType = typeof(TJob);
            createJob.Run();

            return (TJob)createJob.JobInstance;
        }

        public static TJob Job<TJob>(Action<TJob> run)
        {
            var interceptor = new RunInterceptor(invocation => run((TJob)invocation.InvocationTarget));

            var createJob = Simpler.Task.New<CreateTask>();
            createJob.JobType = typeof(TJob);
            createJob.RunInterceptor = interceptor;
            createJob.Run();

            return (TJob)createJob.JobInstance;
        }
    }
}