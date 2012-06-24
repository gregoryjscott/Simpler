using System;
using Simpler.Proxy.Jobs;

namespace Simpler
{
    public class It<TJob> where TJob : Job
    {
        public static void Should(string expectation, Action<TJob> action)
        {
            var createJob = new CreateJob { JobType = typeof(TJob) };
            createJob.Run();
            var job = (TJob)createJob.JobInstance;

            try
            {
                action(job);
                Console.WriteLine("    can " + expectation);
            }
            catch
            {
                Console.WriteLine("    FAILED to " + expectation);
                throw;
            }
        }

        // TODO - get rid of this
        public static void ShouldThrow<TException>(string when, Action<TJob> action)
        {
            var createJob = new CreateJob { JobType = typeof(TJob) };
            createJob.Run();
            var job = (TJob)createJob.JobInstance;

            var expectedExpection = typeof (TException).FullName;
            Console.WriteLine(job.Name);
            try
            {
                action(job);

                Console.WriteLine(String.Format("    FAILED to throw {0} when {1}.", expectedExpection, when));
                throw new SimplerException(String.Format("Test expected {0} to be thrown when {1}.", expectedExpection, when));
            }
            catch (Exception exception)
            {
                if (exception.GetType().FullName == expectedExpection)
                {
                    Console.WriteLine(String.Format("    did throw {0} as expected when {1}.", expectedExpection, when));                    
                }
                else
                {
                    Console.WriteLine(
                        String.Format("    FAILED to throw {0} as expected when {1}, but {2} was thrown instead.",
                                      expectedExpection,
                                      when,
                                      exception.GetType().FullName));

                    throw new SimplerException(
                        String.Format("Test expected {0} to be thrown {1}, but {2} was thrown instead.",
                                      expectedExpection,
                                      when,
                                      exception.GetType().FullName));
                }
            }
            Console.WriteLine("");
        }
    }
}