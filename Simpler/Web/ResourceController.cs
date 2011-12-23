using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simpler.Web.Tasks;

namespace Simpler.Web
{
    /// <summary>
    /// Provides for executing Simpler tasks for a RESTful based controller.
    /// </summary>
    public class ResourceController : Controller
    {
        public ResourceController()
        {
            var findTasks = TaskFactory<FindResourceTasks>.Create();
            findTasks.ControllerType = GetType();
            findTasks.Execute();

            IndexTask = findTasks.Tasks.FirstOrDefault(t => t.GetType().Name.StartsWith("Index"));
            ShowTask = findTasks.Tasks.FirstOrDefault(t => t.GetType().Name.StartsWith("Show"));
            NewTask = findTasks.Tasks.FirstOrDefault(t => t.GetType().Name.StartsWith("New"));
            CreateTask = findTasks.Tasks.FirstOrDefault(t => t.GetType().Name.StartsWith("Create"));
            EditTask = findTasks.Tasks.FirstOrDefault(t => t.GetType().Name.StartsWith("Edit"));
            UpdateTask = findTasks.Tasks.FirstOrDefault(t => t.GetType().Name.StartsWith("Update"));
            DeleteTask = findTasks.Tasks.FirstOrDefault(t => t.GetType().Name.StartsWith("Delete"));
            DestroyTask = findTasks.Tasks.FirstOrDefault(t => t.GetType().Name.StartsWith("Destroy"));
        }

        Task IndexTask { get; set; }
        Task ShowTask { get; set; }
        Task NewTask { get; set; }
        Task CreateTask { get; set; }
        Task EditTask { get; set; }
        Task UpdateTask { get; set; }
        Task DeleteTask { get; set; }
        Task DestroyTask { get; set; }

        static ActionResult ExecuteTask(Task task, Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            if (task == null)
            {
                throw new HttpException(404, "HTTP/1.1 404 Not Found");
            }

            task.Inputs = inputs(null);
            task.Execute();
            return outputs(task.Outputs);
        }
        protected ActionResult Index(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(IndexTask, inputs, outputs);
        }
        protected ActionResult Show(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(ShowTask, inputs, outputs);
        }
        protected ActionResult New(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(NewTask, inputs, outputs);
        }
        protected ActionResult Create(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(CreateTask, inputs, outputs);
        }
        protected ActionResult Edit(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(EditTask, inputs, outputs);
        }
        protected ActionResult Update(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(UpdateTask, inputs, outputs);
        }
        protected ActionResult Delete(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(DeleteTask, inputs, outputs);
        }
        protected ActionResult Destroy(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(DestroyTask, inputs, outputs);
        }
    }
}
