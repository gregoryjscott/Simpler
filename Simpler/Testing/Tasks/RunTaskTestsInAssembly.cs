using System;
using System.Linq;
using System.Reflection;
using Simpler.Construction.Tasks;

namespace Simpler.Testing.Tasks
{
    public class RunTaskTestsInAssembly : Task
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
                        (type.GetMethod("Tests") != null)
                        && 
                        type.IsPublic).ToArray();

            var failureCount = 0;
            foreach (var taskType in taskTypes)
            {
                Console.WriteLine(taskType.FullName);

                var typeToCreate = taskType;

                // If this is a generic type then send Object as the types of T.
                var genericArguments = taskType.GetGenericArguments();
                if (genericArguments.Length > 0)
                {
                    var objectTypes = genericArguments.Select(genericArgument => typeof (object)).ToArray();
                    typeToCreate = taskType.MakeGenericType(objectTypes);
                }

                // Create an instance of the task so the tests can be retrieved.
                CreateTask.TaskType = typeToCreate;
                CreateTask.Execute();
                dynamic task = CreateTask.TaskInstance;

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
                        // Arrange
                        taskTest.Setup(task);

                        // Act
                        task.Execute();

                        // Assert
                        taskTest.Verify(task);

                        Console.WriteLine("    " + taskTest.Expectation);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("    Failed: " + taskTest.Expectation);
                        Console.WriteLine("        Error: " + exception.Message);
                        failureCount++;
                    }
                }
            }

            if (failureCount > 0) throw new Exception("There were failures.");
        }
    }
}
