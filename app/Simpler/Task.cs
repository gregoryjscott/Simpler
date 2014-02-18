using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simpler.Core;
using Simpler.Core.Tasks;

namespace Simpler
{
    [InjectTasks]
    public abstract class Task
    {
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

        public Assembly Assembly
        {
            get
            {
                var baseType = GetType().BaseType;
                if (baseType != null) return baseType.Assembly;
                throw new CheckException("Not able to locate the Assembly for the task.");
            }
        }

        Stats _stats;
        public Stats Stats
        {
            get { return _stats ?? (_stats = new Stats {ExecuteDurations = new List<TimeSpan>()}); } 
            set { _stats = value; }
        }

        public abstract void Execute();

        public static T New<T>()
        {
            var taskType = typeof (T);
            var createTask = new CreateTask {In = {TaskType = taskType}};
            createTask.Execute();
            return (T)createTask.Out.TaskInstance;
        }

        #region Helpers

        internal static bool IsSubTask(PropertyInfo property) { return property.PropertyType.IsSubclassOf(typeof(Task)); }

        #endregion
    }
}
