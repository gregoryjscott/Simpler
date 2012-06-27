using System;
using Simpler.Proxy;
using Simpler.Proxy.Jobs;

namespace Simpler
{
    public class Fake
    {
        public static TJob Job<TJob>(Action<TJob> run)
        {
            var interceptor = new RunInterceptor(invocation => run((TJob)invocation.InvocationTarget));

            var createJob = Simpler.Job.New<CreateJob>();
            createJob.JobType = typeof (TJob);
            createJob.RunInterceptor = interceptor;
            createJob.Run();

            return (TJob)createJob.JobInstance;
        }
    }
}