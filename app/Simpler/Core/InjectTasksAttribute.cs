using System;
using System.Collections.Generic;
using Simpler.Core.Tasks;

namespace Simpler.Core
{
    public class InjectTasksAttribute : EventsAttribute
    {
        readonly List<string> _injectedSubTaskPropertyNames = new List<string>();

        public override void BeforeExecute(SimpleTask simpleTask)
        {
            var inject = new InjectSimpleTasks { In = { SimpleTaskContainingSubSimpleTasks = simpleTask } };
            inject.Execute();
            _injectedSubTaskPropertyNames.AddRange(inject.Out.InjectedSubTaskPropertyNames);
        }

        public override void AfterExecute(SimpleTask simpleTask)
        {
            var dispose = new DisposeSimpleTasks { In = { Owner = simpleTask, InjectedTaskNames = _injectedSubTaskPropertyNames.ToArray() } };
            dispose.Execute();
        }

        public override void OnError(SimpleTask simpleTask, Exception exception) { }
    }
}
