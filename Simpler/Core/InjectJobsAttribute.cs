using System;
using System.Collections.Generic;
using Simpler.Core.Tasks;

namespace Simpler.Core
{
    public class InjectTasksAttribute : EventsAttribute
    {
        readonly List<string> _injectedSubTaskPropertyNames = new List<string>();

        public override void BeforeRun(Task task)
        {
            var inject = new InjectTasks { TaskContainingSubTasks = task };
            inject.Run();
            _injectedSubTaskPropertyNames.AddRange(inject.InjectedSubTaskPropertyNames);
        }

        public override void AfterRun(Task task)
        {
            var dispose = new DisposeTasks { Owner = task, InjectedTaskNames = _injectedSubTaskPropertyNames.ToArray() };
            dispose.Run();
        }

        public override void OnError(Task task, Exception exception) { }
    }
}
