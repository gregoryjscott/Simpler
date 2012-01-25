using Simpler.Construction.Tasks;

namespace Simpler
{
    // todo - deprecate

    /// <summary>
    /// TaskFactory is used to instantiate tasks.
    /// </summary>
    /// <typeparam name="T">The type of task that should be created.</typeparam>
    public class TaskFactory<T> where T : Task
    {
        public static T Create()
        {
            var createTask = new CreateTask { TaskType = typeof(T) };
            createTask.Execute();
            return createTask.TaskInstance as T;
        }
    }
}
