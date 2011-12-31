using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simpler.Construction.Tasks;

namespace Simpler.Testing.Tasks
{
    public class RunTaskTestsInAssembly : Task
    {
        // Inputs
        public Assembly AssemblyWithTasks { get; set; }

        // Sub-tasks
        public CreateTask CreateTask { get; set; }

        public override void Execute()
        {
            if (CreateTask == null) CreateTask = new CreateTask();

            //Console.WriteLine();
            Console.WriteLine(string.Format("Scanning assembly {0} for tests.", AssemblyWithTasks.FullName));

            var taskTypes = AssemblyWithTasks.GetTypes()
                .Where(type => type.IsSubclassOf(typeof (Task)) && type.IsPublic)
                .OrderBy(type => type.FullName);

            var tasksWithoutTests = new List<string>();

            var failureCount = 0;
            foreach (var taskType in taskTypes)
            {
                var typeToCreate = taskType;

                // If this is a generic type then send Object as the types of T.
                var genericArguments = taskType.GetGenericArguments();
                if (genericArguments.Length > 0)
                {
                    var objectTypes = genericArguments.Select(genericArgument => typeof(object)).ToArray();
                    typeToCreate = taskType.MakeGenericType(objectTypes);
                }

                // Create an instance of the task so the tests can be retrieved.
                CreateTask.TaskType = typeToCreate;
                CreateTask.Execute();
                dynamic task = CreateTask.TaskInstance;

                if (task.Tests().Length == 0)
                {
                    tasksWithoutTests.Add(taskType.FullName);
                }
                else
                {
                    Console.WriteLine("    " + taskType.FullName);

                    // For each tests, create a new instance of the task and use it
                    // to perform the test.
                    foreach (var taskTest in task.Tests())
                    {
                        Type typeOfTaskTest = taskTest.GetType();
                        genericArguments = typeOfTaskTest.GetGenericArguments();
                        if (genericArguments.Length > 0)
                        {
                            typeToCreate = typeOfTaskTest.GetGenericArguments().Single();
                            CreateTask.TaskType = typeToCreate;
                        }

                        // Create a new instance for each test.
                        CreateTask.Execute();
                        task = CreateTask.TaskInstance;

                        try
                        {
                            taskTest.Run(task);
                            Console.WriteLine("        can " + taskTest.Expectation);
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine("        Failed: " + taskTest.Expectation);
                            Console.WriteLine("            Error: " + exception.Message);
                            failureCount++;
                        }
                    }
                    Console.WriteLine();
                }
            }

            if (tasksWithoutTests.Count() > 0)
            {
                //Console.WriteLine();
                Console.WriteLine("    The following Tasks are missing tests.");
                foreach (var tasksWithoutTest in tasksWithoutTests)
                {
                    Console.WriteLine("        " + tasksWithoutTest);
                }
            }

            if (failureCount > 0) throw new Exception("     There were failures.");
        }
    }
}
