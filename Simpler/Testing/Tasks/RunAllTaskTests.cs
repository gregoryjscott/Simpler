using System;
using System.Linq;
using System.Reflection;
using Simpler.Construction.Tasks;

namespace Simpler.Testing.Tasks
{
    class RunAllTaskTests : Task
    {
        // Sub-tasks
        public CreateTask CreateTask { get; set; }

        public override void Execute()
        {
            if (CreateTask == null) CreateTask = new CreateTask();

            // todo - wil probably need to look in more than just the executing assembly
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var taskTypes =
                Assembly.GetExecutingAssembly().GetTypes().Where(
                    type => type.GetProperty("Tests") != null
                        && type.IsPublic).ToArray();

            var failureCount = 0;
            foreach (var taskType in taskTypes)
            {
                var typeToCreate = taskType;

                // If this is a generic type then send Object as the types of T.
                var genericArguments = taskType.GetGenericArguments();
                if (genericArguments.Length > 0)
                {
                    var objectTypes = genericArguments.Select(genericArgument => typeof (object)).ToArray();
                    typeToCreate = taskType.MakeGenericType(objectTypes);
                }

                // Create the task so the defined tests can be retrieved.
                CreateTask.TaskType = typeToCreate;
                CreateTask.Execute();
                dynamic taskWithTestDefinitions = CreateTask.TaskInstance;

                // For each tests, create a new instance of the task and use it
                // to perform the test.
                foreach (var taskTest in taskWithTestDefinitions.Tests)
                {
                    var task = taskTest.Setup();
                    task.Execute();

                    try
                    {
                        taskTest.Verify(task);
                        Console.WriteLine("Success: " + taskTest.Expectation);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Failed: " + taskTest.Expectation);
                        Console.WriteLine("    Error: " + exception.Message);
                        failureCount++;
                    }
                }
            }

            if (failureCount > 0) throw new Exception("There were failures.");
        }
    }
}
