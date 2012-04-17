using System;
using System.Collections.Generic;
using System.Linq;
using Simpler.Proxy.Jobs;

namespace Simpler
{
    // todo - change syntax to
    // Test.Assembly("Simpler");
    // Test.Namespace("Simpler.Sql.Jobs");
    // Test.Job<FetchSomething>.Can("allow ...", job => { });

    public class Test
    {
        public static void Assembly(string assemblyName)
        {
            var noTests = new List<string>();
            var failures = new List<string>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == assemblyName);

            var jobTypes = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Job)) && type.IsPublic)
                .OrderBy(type => type.FullName);

            foreach (var jobType in jobTypes)
            {
                var typeToCreate = jobType;

                var genericArguments = jobType.GetGenericArguments();
                if (genericArguments.Length > 0)
                {
                    // We only need to call the job's Test() method, so it doesn't matter
                    // what type of generic arguments are passed.
                    var objectTypes = genericArguments
                        .Select(genericArgument => typeof(object)).ToArray();
                    typeToCreate = jobType.MakeGenericType(objectTypes);
                }

                var createJob = new _CreateJob { JobType = typeToCreate };
                createJob.Run();
                var job = (Job)createJob.JobInstance;

                try
                {
                    job.Test();
                }
                catch (NoTestsException)
                {
                    noTests.Add(job.Name);
                }
                catch
                {
                    failures.Add(job.Name);
                }
            }

            if (failures.Any())
            {
                NUnit.Framework.Assert.Fail(String.Format("{0} tests failed.", failures.Count()));
            }

            if (noTests.Any())
            {
                NUnit.Framework.Assert.Inconclusive(String.Format("{0} jobs are missing tests.", noTests.Count()));
            }
        }
    }

    public class Verify : NUnit.Framework.Assert
    {
        public class Job<TJob> where TJob : Job
        {
            public static void Can(string expectation, Action<TJob> action)
            {
                var createJob = new _CreateJob { JobType = typeof(TJob) };
                createJob.Run();
                var job = (TJob)createJob.JobInstance;

                Console.WriteLine(job.Name);
                try
                {
                    action(job);
                    Console.WriteLine("  can " + expectation);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("  failed to " + expectation);
                    throw new Exception("Test failed.", exception);
                }
                Console.WriteLine("");
            }
        }
    }

    public class Test<TJob> where TJob : Job
    {
        private TJob Job { get; set; }

        public static Test<TJob> New()
        {
            var createJob = new _CreateJob {JobType = typeof (TJob)};
            createJob.Run();

            var job = (TJob) createJob.JobInstance;
            var test = new Test<TJob> {Job = job};
            return test;
        }

        public static void Should(string expectation, Action<TJob> action)
        {
            var createJob = new _CreateJob {JobType = typeof (TJob)};
            createJob.Run();
            var job = (TJob) createJob.JobInstance;

            Console.WriteLine(job.Name);
            try
            {
                action(job);
                Console.WriteLine("  does " + expectation);
            }
            catch (Exception exception)
            {
                Console.WriteLine("  failed to " + expectation);
                throw new Exception("Test failed.", exception);
            }
            Console.WriteLine("");
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

        public static void InAssembly(string assemblyName)
        {
            var noTests = new List<string>();
            var failures = new List<string>();

            var assembly = AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == assemblyName);

            var jobTypes = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof (Job)) && type.IsPublic)
                .OrderBy(type => type.FullName);

            foreach (var jobType in jobTypes)
            {
                var typeToCreate = jobType;

                var genericArguments = jobType.GetGenericArguments();
                if (genericArguments.Length > 0)
                {
                    // We only need to call the job's Test() method, so it doesn't matter
                    // what type of generic arguments are passed.
                    var objectTypes = genericArguments
                        .Select(genericArgument => typeof (object)).ToArray();
                    typeToCreate = jobType.MakeGenericType(objectTypes);
                }

                var createJob = new _CreateJob {JobType = typeToCreate};
                createJob.Run();
                var job = (Job) createJob.JobInstance;

                try
                {
                    job.Test();
                }
                catch (NoTestsException)
                {
                    noTests.Add(job.Name);
                }
                catch
                {
                    failures.Add(job.Name);
                }
            }

            if (failures.Any())
            {
                NUnit.Framework.Assert.Fail(String.Format("{0} tests failed.", failures.Count()));
            }

            if (noTests.Any())
            {
                NUnit.Framework.Assert.Inconclusive(String.Format("{0} jobs are missing tests.", noTests.Count()));
            }
        }
    }
}