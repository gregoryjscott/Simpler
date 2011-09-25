using System;
using System.Linq;
using System.Reflection;
using Simpler.Construction.Tasks;

namespace Simpler.Testing.Tasks
{
    class RunTaskTestsInAssembly : Task
    {
        // Inputs
        public Assembly AssemblyContainTasks { get; set; }

        // Sub-tasks
        public CreateTask CreateTask { get; set; }

        public override void Execute()
        {
            if (CreateTask == null) CreateTask = new CreateTask();

            var taskTypes =
                AssemblyContainTasks.GetTypes().Where(
                    type => 
                        type.IsSubclassOf(typeof(Task))
                        &&
                        (type.GetField("Tests") != null || type.GetProperty("Tests") != null)
                        && 
                        type.IsPublic).ToArray();

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
