using System.Collections.Generic;
using Simpler.Construction;
using Simpler.Injection.Jobs;
using System;

namespace Simpler.Injection
{
    /// <summary>
    /// Job attribute that will automatically inject sub-jobs properties on the job before the job is executed.
    /// </summary>
    public class InjectSubJobsAttribute : ExecutionCallbacksAttribute
    {
        readonly List<string> _injectedSubJobPropertyNames = new List<string>();

        public override void BeforeExecute(Job jobBeingExecuted)
        {
            var inject = new InjectSubJobs { JobContainingSubJobs = jobBeingExecuted };
            inject.Execute();
            _injectedSubJobPropertyNames.AddRange(inject.InjectedSubJobPropertyNames);
        }

        public override void AfterExecute(Job jobBeingExecuted)
        {
            var dispose = new DisposeSubJobs { JobContainingSubJobs = jobBeingExecuted, InjectedSubJobPropertyNames = _injectedSubJobPropertyNames.ToArray() };
            dispose.Execute();
        }

        public override void OnError(Job jobBeingExecuted, Exception exception) { }
    }
}
