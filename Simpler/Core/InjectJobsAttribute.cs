using System;
using System.Collections.Generic;
using Simpler.Core.Tasks;

namespace Simpler.Core
{
    public class InjectTasksAttribute : EventsAttribute
    {
        readonly List<string> _injectedSubTaskPropertyNames = new List<string>();

        public override void BeforeExecute(Task task)
        {
            var inject = new InjectTasks { TaskContainingSubTasks = task };
            inject.Execute();
            _injectedSubTaskPropertyNames.AddRange(inject.InjectedSubTaskPropertyNames);
        }

        public override void AfterExecute(Task task)
        {
            var dispose = new DisposeTasks { Owner = task, InjectedTaskNames = _injectedSubTaskPropertyNames.ToArray() };
            dispose.Execute();
        }

        public override void OnError(Task task, Exception exception) { }
    }
}
