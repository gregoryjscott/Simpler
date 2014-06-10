using System;
using System.Reflection;

namespace Simpler.Core.Tasks
{
    public class ExecuteTask : InTask<ExecuteTask.Input>
    {
        public class Input
        {
            public Task Task { get; set; }
            //public Action<Task> ExecuteAction { get; set; }
        }

        public override void Execute()
        {
            var callbackAttributes = Attribute.GetCustomAttributes(In.Task.GetType(), typeof(EventsAttribute));

            var beforeTime = DateTime.Now;
            try
            {
                foreach (var callbackAttribute in callbackAttributes)
                {
                    ((EventsAttribute)callbackAttribute).BeforeExecute(In.Task);
                }

                //In.ExecuteAction(In.Task);
                try
                {
                    In.Task.GetType().GetMethod("BaseExecute").Invoke(In.Task, null);
                }
                catch (TargetInvocationException tie)
                {
                    throw tie.InnerException;
                }

            }
            catch (Exception exception)
            {
                for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                {
                    ((EventsAttribute)callbackAttributes[i]).OnError(In.Task, exception);
                }

                throw;
            }
            finally
            {
                for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                {
                    ((EventsAttribute)callbackAttributes[i]).AfterExecute(In.Task);
                }

                var afterTime = DateTime.Now;
                var duration = afterTime - beforeTime;
                In.Task.Stats.ExecuteDurations.Add(duration);
            }
        }
    }
}