using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simpler.Construction.Tasks;

namespace Simpler.Web.Tasks
{
    public class FindResourceTasks : Task
    {
        // Inputs
        public Type ControllerType { get; set; }

        // Outputs
        public Task[] Tasks { get; set; }

        // Sub-tasks
        public CreateTask CreateTask { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks.
            if (CreateTask == null) CreateTask = new CreateTask();

            var assemblyContainingController = Assembly.GetAssembly(ControllerType);

            var resourceName =
                ControllerType.Name
                    .Substring(0, ControllerType.Name.IndexOf("Controller"));

            var resourceTasksNameSpace =
                ControllerType.FullName
                    .Substring(0, ControllerType.FullName.IndexOf("Controllers."))
                + "Tasks."
                + resourceName;

            var potentialTaskNames =
                new[]
                    {
                        resourceTasksNameSpace + ".Index",
                        resourceTasksNameSpace + ".Show",
                        resourceTasksNameSpace + ".New",
                        resourceTasksNameSpace + ".Create",
                        resourceTasksNameSpace + ".Edit",
                        resourceTasksNameSpace + ".Update",
                        resourceTasksNameSpace + ".Delete",
                        resourceTasksNameSpace + ".Destroy",
                    };

            var resourceTaskTypes = assemblyContainingController.GetTypes()
                .Where(type => potentialTaskNames.Any(resourceTask => resourceTask == type.FullName)).ToArray();

            var taskList = new List<Task>();
            foreach (var resourceTaskType in resourceTaskTypes)
            {
                CreateTask.TaskType = resourceTaskType;
                CreateTask.Execute();
                taskList.Add((Task)(CreateTask.TaskInstance));
            }
            Tasks = taskList.ToArray();
        }
    }
}
