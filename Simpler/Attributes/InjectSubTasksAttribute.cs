using System.Collections.Generic;
using Simpler.Injection.Tasks;

namespace Simpler.Attributes
{
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
    }
}
