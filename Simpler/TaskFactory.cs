using Simpler.Tasks;

namespace Simpler
{
    public class TaskFactory<T> where T : Task
    {
        public static T Create()
        {
            var createInstance = new CreateInstanceOf<T>();
            createInstance.Execute();
            return createInstance.TaskInstance;
        }
    }
}
