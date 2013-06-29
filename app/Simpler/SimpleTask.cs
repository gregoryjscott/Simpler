using System;
using System.Collections.Generic;
using Simpler.Core;
using Simpler.Core.Tasks;
using System.Linq;

namespace Simpler
{
    [InjectTasks]
    public abstract class SimpleTask
    {
        public static T New<T>()
        {
            var invalidTasks = new[] {"InjectSimpleTasks", "DisposeSimpleTasks"};
            var taskType = typeof (T);
            Check.That(!invalidTasks.Contains(taskType.Name), 
                "This SimpleTask type can't be passed to SimpleTask.New because its a Core SimpleTask used by SimpleTask.New.");

            var createTask = new CreateSimpleTask {In = {TaskType = taskType}};
            createTask.Execute();
            return (T)createTask.Out.TaskInstance;
        }

        public virtual string Name
        {
            get
            {
                var baseType = GetType().BaseType;
                return baseType == null
                           ? "Unknown"
                           : String.Format("{0}.{1}", baseType.Namespace, baseType.Name);
            }
        }

        Stats _stats;
        public Stats Stats
        {
            get { return _stats ?? (_stats = new Stats {ExecuteDurations = new List<TimeSpan>()}); }
            set { _stats = value; }
        }

        public abstract void Execute();
    }
}