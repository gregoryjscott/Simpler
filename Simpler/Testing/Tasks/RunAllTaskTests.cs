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
                    type => type.IsSubclassOf(typeof (Task))).ToArray();

            foreach (var taskType in taskTypes)
            {
                // Create the task so the defined tests can be retrieved.
                CreateTask.TaskType = taskType;
                CreateTask.Execute();
                var taskWithTestDefinitions = (Task)CreateTask.TaskInstance;
                var tests = taskWithTestDefinitions.DefineTests();

                // For each tests, create a new instance of the task and use it
                // to perform the test.
                foreach (var taskTest in tests)
                {
                    CreateTask.TaskType = taskType;
                    CreateTask.Execute();
                    var task = (Task) CreateTask.TaskInstance;

                    // todo - output the taskTest.Expectation
                    
                    taskTest.Setup(task);
                    task.Execute();
                    taskTest.Verify(task);
                }
            }
        }
    }
}
