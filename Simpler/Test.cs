using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simpler.Proxy.Jobs;

namespace Simpler
{
    public class Test
    {
        static void TestAssembly(Assembly assembly, List<string> noTests, List<string> failures)
        {
            var jobTypes = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Job)) && type.IsPublic && !type.Name.Contains("Proxy"))
                .OrderBy(type => type.FullName);

            var count = jobTypes.Count();
            if (count > 0)
            {
                const string message = "Testing assembly {0}, that contains {1} jobs.";
                Console.WriteLine(String.Format(message, assembly.FullName, count));
            }

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

                Console.WriteLine("  " + job.Name);
                try
                {
                    job.Test();
                }
                catch (NoTestsException)
                {
                    Console.WriteLine("    CAN'T DO ANYTHING? (This job is missing specs.)");
                    noTests.Add(job.Name);
                }
                catch
                {
                    failures.Add(job.Name);
                }
                Console.WriteLine("");
            }
        }

        public static void Everything()
        {
            var noTests = new List<string>();
            var failures = new List<string>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                TestAssembly(assembly, noTests, failures);
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

        public static void Assembly(string assemblyName)
        {
            var noTests = new List<string>();
            var failures = new List<string>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = assemblies.Single(a => a.GetName().Name == assemblyName);

            TestAssembly(assembly, noTests, failures);

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