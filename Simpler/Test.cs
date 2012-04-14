using System;
using Simpler.Proxy.Jobs;

namespace Simpler
{
    public class Test<TJob> where TJob : Job
    {
        TJob Job { get; set; }

        public static Test<TJob> New()
        {
            var createJob = new CreateJob { JobType = typeof(TJob) };
            createJob.Run();

            var job = (TJob)createJob.JobInstance;
            var test = new Test<TJob> { Job = job };
            return test;
        }

        public Test<TJob> Arrange(Action<TJob> arrange)
        {
            arrange(Job);
            return this;
        }

        public Test<TJob> Act()
        {
            Job.Run();
            return this;
        }

        public void Assert(Action<TJob> assert)
        {
            assert(Job);
        }
    }
}