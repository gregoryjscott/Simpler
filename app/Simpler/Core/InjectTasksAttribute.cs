using System;
using System.Collections.Generic;
using Simpler.Core.Tasks;

namespace Simpler.Core
{
    public class InjectTasksAttribute : EventsAttribute
    {
        readonly List<string> _injectedTaskPropertyNames = new List<string>();

        public override void BeforeExecute(T task)
        {
            var inject = new InjectTasks { In = { Task = task } };
            inject.Execute();
            _injectedTaskPropertyNames.AddRange(inject.Out.InjectedTaskPropertyNames);
        }

        public override void AfterExecute(T task)
        {
            var dispose = new DisposeTasks { In = { Owner = task, InjectedTaskNames = _injectedTaskPropertyNames.ToArray() } };
            dispose.Execute();
        }

        public override void OnError(T task, Exception exception) { }
    }
}
