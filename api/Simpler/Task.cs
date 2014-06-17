using System;
using System.Collections.Generic;
using Simpler.Core;
using Simpler.Core.Tasks;
using System.Linq;

namespace Simpler
{
    [InjectTasks]
    public abstract class Task
    {
        public static T New<T>()
        {
            var invalidTasks = new[] { "InjectTasks", "DisposeTasks" };
            var taskType = typeof(T);
            if (invalidTasks.Contains(taskType.Name))
            {
                throw new ArgumentException("This task type can't be passed to Task.New because its a Core task used by Task.New.");
            }

            var createTask = new CreateTask { In = { TaskType = taskType } };
            createTask.Execute();
            return (T)createTask.Out.TaskInstance;
        }

        //public Action<Task> ExecuteAction;

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
            get { return _stats ?? (_stats = new Stats { ExecuteDurations = new List<TimeSpan>() }); }
            set { _stats = value; }
        }

        public abstract void Execute();
    }
}