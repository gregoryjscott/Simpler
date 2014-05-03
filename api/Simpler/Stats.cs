using System;
using System.Collections.Generic;

namespace Simpler
{
    public class Stats
    {
        public int ExecuteCount { get { return ExecuteDurations.Count; } }
        public List<TimeSpan> ExecuteDurations { get; set; }
    }
}