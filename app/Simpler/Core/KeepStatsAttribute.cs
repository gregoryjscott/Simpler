using System;

namespace Simpler.Core
{
    public class KeepStatsAttribute : EventsAttribute
    {
        DateTime _beforeTime;

        public override void BeforeExecute(Task task)
        {
            _beforeTime = DateTime.Now;
        }

        public override void AfterExecute(Task task)
        {
            var afterTime = DateTime.Now;
            var duration = afterTime - _beforeTime;
            task.Stats.ExecuteDurations.Add(duration);
        }

        public override void OnError(Task task, Exception exception) { }
    }
}
