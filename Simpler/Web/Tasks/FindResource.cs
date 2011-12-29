using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simpler.Construction.Tasks;

namespace Simpler.Web.Tasks
{
    /// <summary>
    /// Builds a Resource by looking for tasks in a Tasks directory that sits next to the
    /// Models and Views folder corresponding to a Controller.  The ControllerType
    /// is provided and used to find the task classes and build the Resource.
    /// </summary>
    public class FindResource : Task
    {
        // Inputs
        public Type ControllerType { get; set; }

        // Outputs
        public Resource Resource { get; set; }

        // Sub-tasks
        public CreateTask CreateTask { get; set; }

        public override void Execute()
        {
            // Manually create the sub-tasks, for now.
            if (CreateTask == null) CreateTask = new CreateTask();

            var assemblyContainingController = Assembly.GetAssembly(ControllerType);

            var resourceName =
                ControllerType.Name
                    .Substring(0, ControllerType.Name.IndexOf("Controller"));

            var assumedNamespace =
                ControllerType.FullName
                    .Substring(0, ControllerType.FullName.IndexOf("Controllers."))
                + "Tasks."
                + resourceName;

            var potentialTaskNames =
                new[]
                    {
                        assumedNamespace + ".Index",
                        assumedNamespace + ".Show",
                        assumedNamespace + ".New",
                        assumedNamespace + ".Create",
                        assumedNamespace + ".Edit",
                        assumedNamespace + ".Update",
                        assumedNamespace + ".Delete",
                        assumedNamespace + ".Destroy",
                    };

            var resourceTaskTypes = assemblyContainingController.GetTypes()
                .Where(type => potentialTaskNames.Any(resourceTask => resourceTask == type.FullName)).ToArray();

            var taskInstanceList = new List<Task>();
            foreach (var resourceTaskType in resourceTaskTypes)
            {
                CreateTask.TaskType = resourceTaskType;
                CreateTask.Execute();
                taskInstanceList.Add((Task)(CreateTask.TaskInstance));
            }

            // The .StartsWith used below is because Simpler creates proxies, and therefore when you expect a
            // class name of "Index" you actually get a class name of "IndexProxy".
            Resource =
                new Resource
                    {
                        Index = taskInstanceList.FirstOrDefault(t => t.GetType().Name.StartsWith("Index")),
                        Show = taskInstanceList.FirstOrDefault(t => t.GetType().Name.StartsWith("Show")),
                        New = taskInstanceList.FirstOrDefault(t => t.GetType().Name.StartsWith("New")),
                        Create = taskInstanceList.FirstOrDefault(t => t.GetType().Name.StartsWith("Create")),
                        Edit = taskInstanceList.FirstOrDefault(t => t.GetType().Name.StartsWith("Edit")),
                        Update = taskInstanceList.FirstOrDefault(t => t.GetType().Name.StartsWith("Update")),
                        Delete = taskInstanceList.FirstOrDefault(t => t.GetType().Name.StartsWith("Delete")),
                        Destroy = taskInstanceList.FirstOrDefault(t => t.GetType().Name.StartsWith("Destroy"))
                    };
        }
    }
}
