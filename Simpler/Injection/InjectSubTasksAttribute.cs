using System.Collections.Generic;
using Simpler.Construction;
using Simpler.Injection.Tasks;
using System;

namespace Simpler.Injection
{
    /// <summary>
    /// Task attribute that will automatically inject sub-tasks properties on the task before the task is executed.
    /// </summary>
    public class InjectSubTasksAttribute : ExecutionCallbacksAttribute
    {
        readonly List<string> _injectedSubTaskPropertyNames = new List<string>();

        public override void BeforeExecute(Task taskBeingExecuted)
        {
            var inject = new InjectSubTasks { TaskContainingSubTasks = taskBeingExecuted };
            inject.Execute();
            _injectedSubTaskPropertyNames.AddRange(inject.InjectedSubTaskPropertyNames);
        }

        public override void AfterExecute(Task taskBeingExecuted)
        {
            var dispose = new DisposeSubTasks { TaskContainingSubTasks = taskBeingExecuted, InjectedSubTaskPropertyNames = _injectedSubTaskPropertyNames.ToArray() };
            dispose.Execute();
        }

        public override void OnError(Task taskBeingExecuted, Exception exception) { }
    }
}
