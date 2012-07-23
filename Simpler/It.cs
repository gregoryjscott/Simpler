using System;
using Simpler.Core.Tasks;

namespace Simpler
{
    public class It<TTask> where TTask : Task
    {
        public static void Should(string expectation, Action<TTask> action)
        {
            var createTask = new CreateTask { TaskType = typeof(TTask) };
            createTask.Run();
            var job = (TTask)createTask.TaskInstance;

            try
            {
                action(job);
                Console.WriteLine("    can " + expectation);
            }
            catch
            {
                Console.WriteLine("    FAILED to " + expectation);
                throw;
            }
        }
    }
}