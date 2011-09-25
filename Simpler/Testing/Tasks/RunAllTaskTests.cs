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

            var taskTypes =
                Assembly.GetCallingAssembly().GetTypes().Where(
                    type => type.GetProperty("Tests") != null
                        && !type.ContainsGenericParameters
                        && type.IsPublic).ToArray();

            foreach (var taskType in taskTypes)
            {
                //var genericArguments = taskType.GetGenericArguments();

                // Create the task so the defined tests can be retrieved.
                CreateTask.TaskType = taskType;
                CreateTask.Execute();
                dynamic taskWithTestDefinitions = CreateTask.TaskInstance;

                // For each tests, create a new instance of the task and use it
                // to perform the test.
                foreach (var taskTest in taskWithTestDefinitions.Tests)
                {
                    CreateTask.TaskType = taskType;
                    CreateTask.Execute();
                    dynamic task = CreateTask.TaskInstance;

                    taskTest.Setup(task);
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
                    }
                }
            }
        }
    }
}
