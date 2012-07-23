using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simpler.Core.Tasks;

namespace Simpler
{
    public class Describe
    {
        public static void Assembly(string assemblyName)
        {
            var noSpecs = new List<string>();
            var failures = new List<string>();

            var assembly = AppDomain.CurrentDomain.Load(new AssemblyName(assemblyName));
            DescribeAssembly(assembly, noSpecs, failures);

            if (failures.Any())
            {
                NUnit.Framework.Assert.Fail(String.Format("{0} jobs failed.", failures.Count()));
            }

            if (noSpecs.Any())
            {
                NUnit.Framework.Assert.Inconclusive(String.Format("{0} jobs are missing scecs.", noSpecs.Count()));
            }
        }

        public static void Job<T>() where T : Task
        {
            DescribeJob(typeof(T));
        }

        static void DescribeAssembly(Assembly assembly, List<string> noSpecs, List<string> failures)
        {
            var jobTypes = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof (Task))
                    && type.IsPublic
                    && !type.IsAbstract
                    && !type.Name.Contains("Proxy"))
                .OrderBy(type => type.FullName);

            var count = jobTypes.Count();
            if (count > 0)
            {
                const string message = "{0} contains {1} jobs:";
                Console.WriteLine(String.Format(message, assembly.GetName().Name, count));
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
                        .Select(genericArgument => typeof (object)).ToArray();
                    typeToCreate = jobType.MakeGenericType(objectTypes);
                }

                try
                {
                    Console.WriteLine("");
                    DescribeJob(typeToCreate);
                }
                catch (NoSpecsException)
                {
                    noSpecs.Add(typeToCreate.Name);
                }
                catch
                {
                    failures.Add(typeToCreate.Name);
                }
            }
        }

        static void DescribeJob(Type jobType)
        {
            var createJob = new CreateTask {JobType = jobType};
            createJob.Run();
            var job = (Task) createJob.JobInstance;

            Console.WriteLine("  " + job.Name);
            try
            {
                job.Specs();
            }
            catch (NoSpecsException)
            {
                Console.WriteLine("    CAN'T DO ANYTHING? (This job is missing specs.)");
                throw;
            }
        }
    }
}