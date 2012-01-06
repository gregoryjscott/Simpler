using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simpler.Web.Tasks;

namespace Simpler.Web
{
    /// <summary>
    /// Provides for creating a Controller that connects to a Resource.  This allows for very thin controllers
    /// that simply define the routes and delegate all action functionality to Resource Tasks.
    /// </summary>
    public abstract class ResourceController : Controller
    {
        /// <summary>
        /// This constructor should be used when creating a subclass.  Create tasks named Index, Show, New, Create,
        /// Edit, Update, Delete, and/or Destroy in a Task directory next to the Models and Views directories and then
        /// use the Index(), Show(), etc. helper methods to hand off inputs and receive outputs.
        /// </summary>
        protected ResourceController()
        {
            var findResource = TaskFactory<FindResource>.Create();
            findResource.ControllerType = GetType();
            findResource.Execute();
            Resource = findResource.Resource;
        }

        /// <summary>
        /// This constructor should only be used to override the default behavior that automatically builds the
        /// Resource.
        /// </summary>
        /// <param name="resource">The Resource to use instead of the normally automatically created Resource.</param>
        protected ResourceController(Resource resource)
        {
            Resource = resource;
        }

        protected Resource Resource { get; set; }

        static ActionResult ExecuteTask(dynamic task, Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            if (task == null)
            {
                throw new HttpException(404, "HTTP/1.1 404 Not Found");
            }

            task.SetInputs(inputs(null));
            task.Execute();
            return outputs(task.GetOutputs());
        }

        /// <summary>
        /// Helper method that executes the Index task tied to this Controller.
        /// </summary>
        /// <param name="inputs">Lamda function that returns the Inputs that will be passed along to the Index task.</param>
        /// <param name="outputs">Lamda function that returns an ActionResult using the provided Index task Outputs.</param>
        /// <returns>The ActionResult that should be returned from the action.</returns>
        protected ActionResult Index(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(Resource.Index, inputs, outputs);
        }

        /// <summary>
        /// Helper method that executes the Show task tied to this Controller.
        /// </summary>
        /// <param name="inputs">Lamda function that returns the Inputs that will be passed along to the Show task.</param>
        /// <param name="outputs">Lamda function that returns an ActionResult using the provided Show task Outputs.</param>
        /// <returns>The ActionResult that should be returned from the action.</returns>
        protected ActionResult Show(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(Resource.Show, inputs, outputs);
        }

        /// <summary>
        /// Helper method that executes the New task tied to this Controller.
        /// </summary>
        /// <param name="inputs">Lamda function that returns the Inputs that will be passed along to the New task.</param>
        /// <param name="outputs">Lamda function that returns an ActionResult using the provided New task Outputs.</param>
        /// <returns>The ActionResult that should be returned from the action.</returns>
        protected ActionResult New(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(Resource.New, inputs, outputs);
        }

        /// <summary>
        /// Helper method that executes the Create task tied to this Controller.
        /// </summary>
        /// <param name="inputs">Lamda function that returns the Inputs that will be passed along to the Create task.</param>
        /// <param name="outputs">Lamda function that returns an ActionResult using the provided Create task Outputs.</param>
        /// <returns>The ActionResult that should be returned from the action.</returns>
        protected ActionResult Create(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(Resource.Create, inputs, outputs);
        }

        /// <summary>
        /// Helper method that executes the Edit task tied to this Controller.
        /// </summary>
        /// <param name="inputs">Lamda function that returns the Inputs that will be passed along to the Edit task.</param>
        /// <param name="outputs">Lamda function that returns an ActionResult using the provided Edit task Outputs.</param>
        /// <returns>The ActionResult that should be returned from the action.</returns>
        protected ActionResult Edit(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(Resource.Edit, inputs, outputs);
        }

        /// <summary>
        /// Helper method that executes the Update task tied to this Controller.
        /// </summary>
        /// <param name="inputs">Lamda function that returns the Inputs that will be passed along to the Update task.</param>
        /// <param name="outputs">Lamda function that returns an ActionResult using the provided Update task Outputs.</param>
        /// <returns>The ActionResult that should be returned from the action.</returns>
        protected ActionResult Update(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(Resource.Update, inputs, outputs);
        }

        /// <summary>
        /// Helper method that executes the Delete task tied to this Controller.
        /// </summary>
        /// <param name="inputs">Lamda function that returns the Inputs that will be passed along to the Delete task.</param>
        /// <param name="outputs">Lamda function that returns an ActionResult using the provided Delete task Outputs.</param>
        /// <returns>The ActionResult that should be returned from the action.</returns>
        protected ActionResult Delete(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(Resource.Delete, inputs, outputs);
        }

        /// <summary>
        /// Helper method that executes the Destroy task tied to this Controller.
        /// </summary>
        /// <param name="inputs">Lamda function that returns the Inputs that will be passed along to the Destroy task.</param>
        /// <param name="outputs">Lamda function that returns an ActionResult using the provided Destroy task Outputs.</param>
        /// <returns>The ActionResult that should be returned from the action.</returns>
        protected ActionResult Destroy(Func<dynamic, dynamic> inputs, Func<dynamic, ActionResult> outputs)
        {
            return ExecuteTask(Resource.Destroy, inputs, outputs);
        }
    }
}
