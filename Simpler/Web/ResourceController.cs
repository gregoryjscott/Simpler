using System;
using System.Web.Mvc;

namespace Simpler.Web
{
    /// <summary>
    /// Provides for executing Simpler tasks for a RESTful based controller.
    /// </summary>
    public class ResourceController : Controller
    {
        protected Task IndexTask { get; set; }
        protected Task ShowTask { get; set; }
        protected Task NewTask { get; set; }
        protected Task CreateTask { get; set; }
        protected Task EditTask { get; set; }
        protected Task UpdateTask { get; set; }
        protected Task DeleteTask { get; set; }
        protected Task DestroyTask { get; set; }

        static ActionResult ExecuteTask(Task task, Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            // TODO - check to see if task is null

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
