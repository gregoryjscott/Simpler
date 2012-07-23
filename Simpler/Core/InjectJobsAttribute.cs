using System;
using System.Collections.Generic;
using Simpler.Core.Tasks;

namespace Simpler.Core
{
    public class InjectJobsAttribute : EventsAttribute
    {
        readonly List<string> _injectedSubJobPropertyNames = new List<string>();

        public override void BeforeRun(Task task)
        {
            var inject = new InjectTasks { TaskContainingSubTasks = task };
            inject.Run();
            _injectedSubJobPropertyNames.AddRange(inject.InjectedSubJobPropertyNames);
        }

        public override void AfterRun(Task task)
        {
            var dispose = new DisposeTasks { Owner = task, InjectedJobNames = _injectedSubJobPropertyNames.ToArray() };
            dispose.Run();
        }

        public override void OnError(Task task, Exception exception) { }
    }
}
