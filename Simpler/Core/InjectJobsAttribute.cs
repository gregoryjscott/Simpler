using System;
using System.Collections.Generic;
using Simpler.Core.Jobs;

namespace Simpler.Core
{
    public class InjectJobsAttribute : EventsAttribute
    {
        readonly List<string> _injectedSubJobPropertyNames = new List<string>();

        public override void BeforeRun(Job job)
        {
            var inject = new InjectJobs { JobContainingSubJobs = job };
            inject.Run();
            _injectedSubJobPropertyNames.AddRange(inject.InjectedSubJobPropertyNames);
        }

        public override void AfterRun(Job job)
        {
            var dispose = new DisposeJobs { Owner = job, InjectedJobNames = _injectedSubJobPropertyNames.ToArray() };
            dispose.Run();
        }

        public override void OnError(Job job, Exception exception) { }
    }
}
