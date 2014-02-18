using System;
using System.Threading.Tasks;

namespace Simpler
{
    public static class Execute
    {
        public static TTask Now<TTask>(Action<TTask> setup = null) where TTask : Task
        {
            var task = Task.New<TTask>();
            if (setup != null) setup(task);
            task.Execute();
            return task;
        }

        public static Task<TTask> Async<TTask>(Action<TTask> setup = null) where TTask : Task
        {
            return System.Threading.Tasks.Task.Factory.StartNew(() => Now(setup));
        }
    }
}
