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
                NUnit.Framework.Assert.Fail(String.Format("{0} tasks failed.", failures.Count()));
            }

            if (noSpecs.Any())
            {
                NUnit.Framework.Assert.Inconclusive(String.Format("{0} tasks are missing specs.", noSpecs.Count()));
            }
        }

        public static void Task<T>() where T : Task
        {
            DescribeTask(typeof(T));
        }

        static void DescribeAssembly(Assembly assembly, List<string> noSpecs, List<string> failures)
        {
            var taskTypes = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof (Task))
                    && type.IsPublic
                    && !type.IsAbstract
                    && !type.Name.Contains("Proxy"))
                .OrderBy(type => type.FullName);

            var count = taskTypes.Count();
            if (count > 0)
            {
                const string message = "{0} contains {1} tasks:";
                Console.WriteLine(String.Format(message, assembly.GetName().Name, count));
            }

            foreach (var taskType in taskTypes)
            {
                var typeToCreate = taskType;

                var genericArguments = taskType.GetGenericArguments();
                if (genericArguments.Length > 0)
                {
                    // We only need to call the task's Test() method, so it doesn't matter
                    // what type of generic arguments are passed.
                    var objectTypes = genericArguments
                        .Select(genericArgument => typeof (object)).ToArray();
                    typeToCreate = taskType.MakeGenericType(objectTypes);
                }

                try
                {
                    Console.WriteLine("");
                    DescribeTask(typeToCreate);
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

        static void DescribeTask(Type taskType)
        {
            var createTask = new CreateTask {TaskType = taskType};
            createTask.Run();
            var task = (Task) createTask.TaskInstance;

            Console.WriteLine("  " + task.Name);
            try
            {
                task.Specs();
            }
            catch (NoSpecsException)
            {
                Console.WriteLine("    CAN'T DO ANYTHING? (This task is missing specs.)");
                throw;
            }
        }
    }
}